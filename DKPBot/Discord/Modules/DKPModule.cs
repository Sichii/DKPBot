using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chaos.Core.Extensions;
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
        private readonly AliasService AliasService;
        private readonly SettingsService SettingsService;
        protected override Logger Log { get; }

        public DKPModule(EQDKPService eqDkpService, AliasService aliasService, SettingsService settingsService)
        {
            EQDKPService = eqDkpService;
            AliasService = aliasService;
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
                var aliases = await AliasService.Aliases.Where(alias => alias.Result.EqualsI(characterNameOrClass))
                    .Select(alias => alias.Original)
                    .ToListAsync();

                aliases.Add(characterNameOrClass);
                aliases = aliases.Distinct()
                    .ToList();

                async Task DkpForNameOrClass(string nameOrClass)
                {
                    await foreach ((var name, var points) in EQDKPService
                        .GetPointsForCharacter(nameOrClass, SettingsService.DKPPoolName)
                        .OrderByDescending(entry => entry.Points))
                        characterStrBuilder.AppendLine($@"{name}: {points}");

                    Log.Trace($@"Fetching dkp for {nameOrClass}");
                    if (Enum.TryParse(nameOrClass, true, out EQClassFlags classFlag))
                    {
                        await foreach ((var name, var points) in EQDKPService.GetPointsForClass(classFlag, SettingsService.DKPPoolName)
                            .OrderByDescending(entry => entry.Points))
                            classStrBuilder.AppendLine($@"{name}: {points}");

                        embedBuilder.AddField($@"{classFlag}", classStrBuilder.ToString());
                    }
                }

                await Task.WhenAll(aliases.Select(DkpForNameOrClass));

                if (characterStrBuilder.Length > 0)
                    embedBuilder.AddField($@"Character(s)", characterStrBuilder.ToString());

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