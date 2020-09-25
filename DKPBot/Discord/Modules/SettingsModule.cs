using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DKPBot.Definitions;
using DKPBot.Discord.Attributes;
using DKPBot.Services;
using NLog;

namespace DKPBot.Discord.Modules
{
    public class SettingsModule : ModuleBase
    {
        private readonly SettingsService SettingsService;
        protected override Logger Log { get; }

        public SettingsModule(SettingsService settingsService)
        {
            Log = settingsService.Log;
            SettingsService = settingsService;
        }

        [Command("setPrefix", RunMode = RunMode.Async), Summary("Sets the bot prefix for commands."), RequirePrivilege(Privilege.Elevated)]
        public async Task SetPrefix(string prefix)
        {
            prefix = prefix.Trim();
            if (prefix.Contains(' '))
            {
                await ReplyAsync("Prefix cannot have spaces in it.");
                return;
            }

            SettingsService.Prefix = prefix;
            await SettingsService.SerializeAsync();
            await Context.Message.AddReactionAsync(new Emoji("👌"));
        }
    }
}