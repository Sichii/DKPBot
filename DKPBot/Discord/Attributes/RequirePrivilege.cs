using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using DKPBot.Definitions;

namespace DKPBot.Discord.Attributes
{
    /// <summary>
    ///     Required a user to have certain privileges to execute a command or module.
    /// </summary>
    internal class RequirePrivilege : PreconditionAttributeBase
    {
        private readonly Privilege Privilege;

        internal RequirePrivilege(Privilege privilege) => Privilege = privilege;

        public override Task<PreconditionResult> CheckPermissionsAsync(
            ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var user = (SocketGuildUser) context.User;
            ErrorMessage =
                $"{user.Username} does not have the required privilege({Privilege}) to run this command({command.Name}).";

            return Privilege switch
            {
                Privilege.None          => Success,
                Privilege.Normal        => (user.GuildPermissions.SendMessages && user.GuildPermissions.Connect ? Success : Error),
                Privilege.Elevated      => (user.GuildPermissions.ManageChannels || user.GuildPermissions.KickMembers ? Success : Error),
                Privilege.Administrator => (user.GuildPermissions.Administrator ? Success : Error),
                _                       => throw new ArgumentOutOfRangeException()
            };
        }
    }
}