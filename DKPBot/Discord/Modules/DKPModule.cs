using System;
using System.Threading.Tasks;
using Discord.Commands;
using DKPBot.Definitions;
using DKPBot.Discord.Attributes;
using DKPBot.Services;
using NLog;

namespace DKPBot.Discord.Modules
{
    public class DKPModule : ModuleBase
    {
        private readonly EQDKPService EQDKPService;
        private readonly SettingsService SettingsService;
        protected override Logger Log { get; }

        public DKPModule(EQDKPService eqDkpService, SettingsService settingsService)
        {
            EQDKPService = eqDkpService;
            SettingsService = settingsService;
            Log = eqDkpService.Log;
        }

        [Command("dkp", RunMode = RunMode.Async), Summary("Gets the current dkp for a given character name, or partial name"),
         RequirePrivilege(Privilege.Normal)]
        public async Task DKP(string characterName)
        {
            try
            {
                if (string.IsNullOrEmpty(SettingsService.DKPPoolName))
                    await ReplyAsync($@"Set DKP-Pool Name first");

                Log.Trace($@"Fetching dkp for {characterName}");
                (var name, var points) = await EQDKPService.GetPoints(characterName, SettingsService.DKPPoolName);

                await ReplyAsync($@"{name} has {points} dkp");
            } catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}