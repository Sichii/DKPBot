using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DKPBot.Definitions;
using DKPBot.Services.EQDKPModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace DKPBot.Services
{
    public class EQDKPService : IGuildService
    {
        private static readonly string EQDKP_PATH = $@"{CONSTANTS.DATA_DIR}\EQDKP.json";
        private readonly Dictionary<string, int> CharacterCache;
        private readonly HttpClient HttpClient;
        public ulong GuildId { get; }
        public Logger Log { get; }

        public EQDKPService(ulong guildId)
        {
            GuildId = guildId;

            //dont fetch for default guild id
            if (guildId == ulong.MaxValue)
                return;

            Log = LogManager.GetLogger($"EQDKP({GuildId})");

            var json = File.ReadAllText(EQDKP_PATH);
            var eqdkp = JsonConvert.DeserializeObject<JObject>(json)[GuildId.ToString()];
            var apiKey = eqdkp.Value<string>("ApiKey");
            var baseUrl = eqdkp.Value<string>("BaseUrl");

            HttpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", apiKey);

            CharacterCache = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var unused = PrefetchCharacterIds();
        }

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

        public async Task<(int Id, string Name)> FindCharacter(string characterName)
        {
            if (!CharacterCache.TryGetValue(characterName, out var characterId))
            {
                //search for member info based on characterName
                using var charResponse = await HttpClient.GetAsync($@"api.php?function=search&in=charname&for={characterName}");
                await using var charInfoStream = await charResponse.Content.ReadAsStreamAsync();
                var charInfo = XmlConvert.DeserializeObject<SearchResponse<Member>>(charInfoStream);

                //if we dont find an exact match, we could get "relevant" matches
                if (charInfo.Relevant == null)
                {
                    //if we get a direct match, cache the character id so we dont have to search for it again
                    characterId = charInfo.Direct.Result.Id;
                    CharacterCache[characterName] = characterId;
                } else
                {
                    //if we only get relevant matches, grab the first and dont cache the ID
                    var first = charInfo.Relevant.Results.First();
                    characterId = first.Id;
                    characterName = first.Name;
                }
            }

            return (characterId, characterName);
        }

        public async Task<(string Name, int Points)> GetPoints(string characterName, string poolName)
        {
            (var id, var name) = await FindCharacter(characterName);

            //find dkp based on character id
            using var pointsResponse = await HttpClient.GetAsync($@"api.php?function=points&filter=character&filterid={id}");
            await using var pointsInfoStream = await pointsResponse.Content.ReadAsStreamAsync();
            var pointsInfo = XmlConvert.DeserializeObject<Response>(pointsInfoStream);

            //find the dkp for the pool specified in the 
            var poolId = pointsInfo.MultidkpPools.MultidkpPool.First(pool => pool.Name.EqualsI(poolName))
                .Id;
            return (name, pointsInfo.Players.Player.First()
                .Points.MultidkpPoints.First(pool => pool.MultidkpId == poolId)
                .PointsCurrent);
        }
    }
}