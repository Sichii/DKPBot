using System.IO;
using System.Threading.Tasks;
using Chaos.Core.Collections.Synchronized.Awaitable;
using DKPBot.Definitions;
using DKPBot.Services.AliasModel;
using Newtonsoft.Json;

namespace DKPBot.Services
{
    [JsonObject]
    public class AliasService : GuildServiceBase, ISerializableGuildService
    {
        [JsonIgnore]
        private readonly string AliasPath;
        [JsonProperty]
        public AwaitableHashSet<Alias> Aliases { get; private set; }

        [JsonConstructor]
        public AliasService(ulong guildId)
            : base(guildId)
        {
            AliasPath = CreateAliasPath(guildId);
            Aliases = new AwaitableHashSet<Alias>();
        }

        public Task SerializeAsync()
        {
            Log.Debug("Saving aliases to file...");
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var dir = Path.GetDirectoryName(AliasPath);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return File.WriteAllTextAsync(AliasPath, json);
        }

        public async Task PopulateAsync()
        {
            if (File.Exists(AliasPath))
            {
                var json = await File.ReadAllTextAsync(AliasPath);
                JsonConvert.PopulateObject(json, this);
            }
        }

        private static string CreateAliasPath(ulong guildId) => $@"{CONSTANTS.DATA_DIR}\{guildId}\aliases.json";
    }
}