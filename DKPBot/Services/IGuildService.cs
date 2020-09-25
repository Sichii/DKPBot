using NLog;

namespace DKPBot.Services
{
    public interface IGuildService
    {
        ulong GuildId { get; }
        Logger Log { get; }
    }
}