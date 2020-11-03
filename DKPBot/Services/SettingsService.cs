using System.IO;
using System.Threading.Tasks;
using DKPBot.Definitions;
using Newtonsoft.Json;
using NLog;

namespace DKPBot.Services
{
    [JsonObject]
    public class SettingsService : IGuildService
    {
        [JsonIgnore]
        private readonly string SettingsPath;

        [JsonProperty]
        public ulong GuildId { get; set; }

        [JsonProperty]
        internal string Prefix { get; set; }

        [JsonProperty]
        internal string DKPPoolName { get; set; }

        [JsonIgnore]
        public Logger Log { get; }

        [JsonConstructor]
        internal SettingsService(ulong guildId)
        {
            Log = LogManager.GetLogger($"Settings({guildId})");
            SettingsPath = CreateSettingsPath(guildId);
            GuildId = guildId;
            Prefix = "!";
        }

        internal Task SerializeAsync()
        {
            Log.Debug("Saving settings to file...");
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            var dir = Path.GetDirectoryName(SettingsPath);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return File.WriteAllTextAsync(SettingsPath, json);
        }

        internal static async Task<SettingsService> CreateAsync(ulong guildId)
        {
            var path = CreateSettingsPath(guildId);
            if (File.Exists(path))
            {
                var json = await File.ReadAllTextAsync(path);
                return JsonConvert.DeserializeObject<SettingsService>(json);
            }

            return new SettingsService(guildId);
        }

        private static string CreateSettingsPath(ulong guildId) => $@"{CONSTANTS.DATA_DIR}\{guildId}\settings.json";
    }
}