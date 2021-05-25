using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Chaos.Core.Extensions;
using DKPBot.Definitions;
using DKPBot.Services.EQDKPModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DKPBot.Services
{
    public class EQDKPService : GuildServiceBase
    {
        private static readonly string EQDKP_PATH = $@"{CONSTANTS.DATA_DIR}\EQDKP.json";
        private readonly Dictionary<string, int> CharacterCache;
        private readonly HttpClient HttpClient;

        public EQDKPService(ulong guildId)
            : base(guildId)
        {
            //dont fetch for default guild id
            if (guildId == ulong.MaxValue)
                return;

            var json = File.ReadAllText(EQDKP_PATH);
            var eqdkp = JsonConvert.DeserializeObject<JObject>(json);

            if (eqdkp.TryGetValue(GuildId.ToString(), out var guildSettings))
            {
                var apiKey = guildSettings.Value<string>("ApiKey");
                var baseUrl = guildSettings.Value<string>("BaseUrl");

                HttpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
                HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", apiKey);

                CharacterCache = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                var unused = PrefetchCharacterIds();
            } else
                Log.Error("EQDKP Service created without required guild settings.");
        }

        public static Task<EQDKPService> CreateAsync(ulong guildId) => Task.FromResult(new EQDKPService(guildId));

        public async Task PrefetchCharacterIds()
        {
            Log.Debug("Prefetching character ids...");
            using var response = await HttpClient.GetAsync(@"api.php?function=points");
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var info = XmlConvert.DeserializeObject<Response>(responseStream);

            foreach (var character in info.Players.Player)
                CharacterCache[character.Name] = character.Id;

            Log.Debug("Prefetch complete.");
        }

        public async IAsyncEnumerable<(int Id, string Name)> FindCharacter(string characterName)
        {
            if (!CharacterCache.TryGetValue(characterName, out var id))
            {
                Log.Debug($@"Searching for character {characterName}...");
                using var charResponse = await HttpClient.GetAsync($@"api.php?function=search&in=charname&for={characterName}");
                await using var charInfoStream = await charResponse.Content.ReadAsStreamAsync();
                var charInfo = XmlConvert.DeserializeObject<SearchResponse<Member>>(charInfoStream);

                if (charInfo.Relevant != null)
                {
                    Log.Debug($@"{charInfo.Relevant.Results.Count} relevant matches found for {characterName}");
                    foreach (var entry in charInfo.Relevant.Results)
                        yield return (entry.Id, entry.Name);
                } else if (charInfo.Direct != null)
                {
                    Log.Debug($@"Direct match found for {characterName}");
                    CharacterCache[charInfo.Direct.Result.Name] = charInfo.Direct.Result.Id;
                    yield return (charInfo.Direct.Result.Id, charInfo.Direct.Result.Name);
                } else
                    Log.Warn($@"No character matches found for {characterName}");
            } else
                yield return (id, characterName);
        }

        public async IAsyncEnumerable<(string Name, int Points)> GetPointsForClass(EQClassFlags classFlags, string poolName)
        {
            Log.Debug($"Searching for class {classFlags}...");
            using var response = await HttpClient.GetAsync(@"api.php?function=points");
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var info = XmlConvert.DeserializeObject<Response>(responseStream);
            var poolId = info.MultidkpPools.MultidkpPool.First(pool => pool.Name.EqualsI(poolName))
                .Id;

            foreach (var player in info.Players.Player)
                if (Enum.TryParse(player.ClassName, true, out EQClassFlags playerClassFlag) && classFlags.HasFlag(playerClassFlag))
                {
                    var dkpPool = player.Points.MultidkpPoints.FirstOrDefault(pool => pool.MultidkpId == poolId);

                    if (player.Active && (dkpPool?.PointsEarned > 0 || dkpPool?.PointsAdjustment > 0))
                        yield return (player.Name, dkpPool.PointsCurrent);
                }
        }

        public IAsyncEnumerable<(string Name, int Points)> GetPointsForCharacter(string characterName, string poolName) =>
            FindCharacter(characterName)
                .Select(async entry =>
                {
                    (var id, var name) = entry;
                    Log.Debug($@"Retreiving dkp for character id {id}...");
                    using var pointsResponse = await HttpClient.GetAsync($@"api.php?function=points&filter=character&filterid={id}");
                    await using var pointsInfoStream = await pointsResponse.Content.ReadAsStreamAsync();
                    var pointsInfo = XmlConvert.DeserializeObject<Response>(pointsInfoStream);

                    //find the dkp for the pool specified in the 
                    var poolId = pointsInfo.MultidkpPools.MultidkpPool.First(innerPool => innerPool.Name.EqualsI(poolName))
                        .Id;
                    var player = pointsInfo.Players.Player.FirstOrDefault();
                    var pool = player?.Points.MultidkpPoints.FirstOrDefault(innerPool => innerPool.MultidkpId == poolId);

                    if (player?.Active == true && (pool?.PointsEarned > 0 || pool?.PointsAdjustment > 0))
                        return (name, points: (int?) pool.PointsCurrent);

                    return default;
                })
                .WhenEach()
                .Where(result => result.points.HasValue)
                .Select(result => (result.name, points: result.points ?? default));
    }
}