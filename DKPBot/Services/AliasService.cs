using System.IO;
using System.Threading.Tasks;
using Chaos.Core.Collections.Synchronized.Awaitable;
using DKPBot.Definitions;
using DKPBot.Services.AliasModel;
using Newtonsoft.Json;
using NLog;

namespace DKPBot.Services
{
    [JsonObject]
    public class AliasService : IGuildService
    {
        [JsonIgnore]
        private readonly string AliasPath;
        [JsonProperty]
        public ulong GuildId { get; }
        [JsonProperty]
        public AwaitableHashSet<Alias> Aliases { get; private set; }
        [JsonIgnore]
        public Logger Log { get; }

        [JsonConstructor]
        internal AliasService(ulong guildId)
        {
            GuildId = guildId;
            Log = LogManager.GetLogger($"Alias({guildId})");
            AliasPath = CreateAliasPath(guildId);
            Aliases = new AwaitableHashSet<Alias>();
        }

        internal Task SerializeAsync()
        {
            Log.Debug("Saving aliases to file...");
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var dir = Path.GetDirectoryName(AliasPath);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return File.WriteAllTextAsync(AliasPath, json);
        }

        internal static async Task<AliasService> CreateAsync(ulong guildId)
        {
            var path = CreateAliasPath(guildId);
            if (File.Exists(path))
            {
                var json = await File.ReadAllTextAsync(path);
                return JsonConvert.DeserializeObject<AliasService>(json);
            }

            return new AliasService(guildId);
        }

        private static string CreateAliasPath(ulong guildId) => $@"{CONSTANTS.DATA_DIR}\{guildId}\aliases.json";
    }
}