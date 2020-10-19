using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DKPBot.Definitions;
using DKPBot.Discord.Attributes;
using DKPBot.Model;
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

        [Command("setPrefix", RunMode = RunMode.Async), Summary("Sets the bot prefix for commands"), RequirePrivilege(Privilege.Elevated)]
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

        [Command("setDkpPoolName", RunMode = RunMode.Async), Summary("Sets the dkp pool name to be used by the dkp service"), RequirePrivilege(Privilege.Elevated)]
        public async Task SetPool(string dkpPoolName)
        {
            dkpPoolName = dkpPoolName.Trim();

            SettingsService.DKPPoolName = dkpPoolName;
            await SettingsService.SerializeAsync();
            await Context.Message.AddReactionAsync(new Emoji("👌"));
        }

        [Command("help", RunMode = RunMode.Async), Summary("The message you're currently reading")]
        public Task Help() =>
            ReplyAsync("Here's a list of commands and their description: ", false, CommandHandler.CreateHelpEmbed(SettingsService));
    }
}