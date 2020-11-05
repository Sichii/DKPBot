using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DKPBot.Services;
using DKPBot.Services.AliasModel;
using NLog;

namespace DKPBot.Discord.Modules
{
    public class AliasModule : ModuleBase
    {
        private readonly AliasService AliasService;
        protected override Logger Log { get; }

        public AliasModule(AliasService aliasService)
        {
            Log = aliasService.Log;
            AliasService = aliasService;
        }

        [Command("alias", RunMode = RunMode.Async), Summary("Sets a new alias, allowing the original to be converted to the result.")]
        public async Task AddAlias(string original, string result)
        {
            var obj = new Alias(original, result);

            await AliasService.Aliases.SyncAssertAsync(async hashSet =>
            {
                hashSet.Add(obj);
                await AliasService.SerializeAsync();
            });

            await Context.Message.AddReactionAsync(Emojis.OK_HAND);
        }

        [Command("rmAlias", RunMode = RunMode.Async), Summary("Removes a previously created alias.")]
        public async Task RemoveAlias(string original, string result)
        {
            var obj = new Alias(original, result);

            var removeResult = await AliasService.Aliases.SyncAssertAsync(async hashSet =>
            {
                if (hashSet.Remove(obj))
                {
                    await AliasService.SerializeAsync();
                    return true;
                }

                return false;
            });

            if(removeResult)
                await Context.Message.AddReactionAsync(Emojis.OK_HAND);
            else
                await Context.Message.AddReactionAsync(Emojis.X);
        }

        [Command("aliases", RunMode = RunMode.Async), Summary("Lists all aliases currently recognized.")]
        public async Task ListAliases()
        {
            if (AliasService.Aliases.Count == 0)
                return;

            var builder = new EmbedBuilder();

            await foreach (var aliasGroup in AliasService.Aliases.GroupBy(alias => alias.Result))
                builder.AddField(aliasGroup.Key, string.Join(", ", aliasGroup.Select(alias => alias.Original).ToEnumerable()));

            await ReplyAsync("Alias list:", false, builder.Build());
        }
    }
}