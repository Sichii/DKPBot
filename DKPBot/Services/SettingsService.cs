using System.IO;
using System.Threading.Tasks;
using DKPBot.Definitions;
using Newtonsoft.Json;

namespace DKPBot.Services
{
    [JsonObject]
    public class SettingsService : GuildServiceBase, ISerializableGuildService
    {
        [JsonIgnore]
        private readonly string SettingsPath;

        [JsonProperty]
        public string Prefix { get; set; }

        [JsonProperty]
        public string DKPPoolName { get; set; }

        [JsonConstructor]
        public SettingsService(ulong guildId)
            : base(guildId)
        {
            SettingsPath = CreateSettingsPath(guildId);
            Prefix = "!";
        }

        public Task SerializeAsync()
        {
            Log.Debug("Saving settings to file...");
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var dir = Path.GetDirectoryName(SettingsPath);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return File.WriteAllTextAsync(SettingsPath, json);
        }

        public async Task PopulateAsync()
        {
            if (File.Exists(SettingsPath))
            {
                var json = await File.ReadAllTextAsync(SettingsPath);
                JsonConvert.PopulateObject(json, this);
            }
        }

        private static string CreateSettingsPath(ulong guildId) => $@"{CONSTANTS.DATA_DIR}\{guildId}\settings.json";
    }
}