using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DKPBot.Definitions;
using DKPBot.Discord.Attributes;
using DKPBot.Services;
using DKPBot.Services.EQDKPModel;
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
        public async Task DKP(string characterNameOrClass)
        {
            try
            {
                if (string.IsNullOrEmpty(SettingsService.DKPPoolName))
                {
                    await ReplyAsync($@"Set DKP-Pool Name first");
                    return;
                }

                var embedBuilder = new EmbedBuilder();
                var characterStrBuilder = new StringBuilder();
                var classStrBuilder = new StringBuilder();

                await foreach ((var name, var points) in EQDKPService
                    .GetPointsForCharacter(characterNameOrClass, SettingsService.DKPPoolName)
                    .OrderByDescending(entry => entry.Points))
                    characterStrBuilder.AppendLine($@"{name}: {points}");

                if (characterStrBuilder.Length > 0)
                    embedBuilder.AddField($@"Character(s): {characterNameOrClass}", characterStrBuilder.ToString());

                Log.Trace($@"Fetching dkp for {characterNameOrClass}");
                if (Enum.TryParse(characterNameOrClass, true, out EQClassFlags classFlag))
                {
                    var className = classFlag.ToString();

                    await foreach ((var name, var points) in EQDKPService.GetPointsForClass(classFlag, SettingsService.DKPPoolName)
                        .OrderByDescending(entry => entry.Points))
                        classStrBuilder.AppendLine($@"{name}: {points}");

                    embedBuilder.AddField($@"Class: {characterNameOrClass}", classStrBuilder.ToString());
                }

                if (embedBuilder.Length == 0)
                    await ReplyAsync($@"No matches found for ""{characterNameOrClass}""");
                else
                    await ReplyAsync($@"Results for ""{characterNameOrClass}""", false, embedBuilder.Build());
            } catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}