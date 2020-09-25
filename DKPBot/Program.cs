using System.Threading.Tasks;
using DKPBot.Model;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace DKPBot
{
    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget("file")
            {
                Layout = new SimpleLayout(
                    @"[${date:format=yyyy-MM-dd HH\:mm\:ss}][${level:uppercase=true}][${logger}]${event-context:item=Command}${event-context:item=RequestedBy} ${message}"),
                FileName = @"logs\DKPBot\${shortdate}.txt",
                ArchiveFileName = @"logs\DKPBot\old\${shortdate}.txt",
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                MaxArchiveFiles = 30
            };
            var consoleTarget = new ConsoleTarget("console")
            {
                Layout =
                    @"[${level:uppercase=true}][${logger}]${event-context:item=Command}${event-context:item=RequestedBy} ${message}",
                WriteBuffer = true
            };

            config.AddTarget(fileTarget);
            config.AddTarget(consoleTarget);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, "file");
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, "console");

            LogManager.Configuration = config;

            await Client.LoginAsync();
            await Task.Delay(-1);
        }
    }
}
