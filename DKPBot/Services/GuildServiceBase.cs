using Newtonsoft.Json;
using NLog;

namespace DKPBot.Services
{
    public abstract class GuildServiceBase : IGuildService
    {
        [JsonProperty]
        public ulong GuildId { get; }
        [JsonIgnore]
        public Logger Log { get; }

        protected GuildServiceBase(ulong guildId)
        {
            GuildId = guildId;
            Log = LogManager.GetLogger(guildId.ToString());
        }
    }
}