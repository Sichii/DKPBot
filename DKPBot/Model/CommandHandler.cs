using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DKPBot.Services;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace DKPBot.Model
{
    /// <summary>
    ///     Handles commands that appear in any server that the bot is part of.
    /// </summary>
    internal static class CommandHandler
    {
        private static readonly Logger Log;
        private static readonly CommandService CommandService;

        static CommandHandler()
        {
            Log = LogManager.GetLogger("Cmd");
            CommandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async
            });
            CommandService.CommandExecuted += CommandService_CommandExecuted;
        }

        internal static Task ConfigureAsync(IServiceProvider provider) => CommandService.AddModulesAsync(Assembly.GetExecutingAssembly(), provider);

        private static Task CommandService_CommandExecuted(Optional<CommandInfo> commandInfo, ICommandContext context, IResult result)
        {
            //print errors
            if (!result.IsSuccess)
                Log.Error($"Guild: {context.Guild.Id} ERROR: {result.ErrorReason}");

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Parse input. If it's a command, executes relevant method.
        /// </summary>
        /// <param name="message">The message to parse.</param>
        internal static async Task TryHandleAsync(SocketMessage message)
        {
            var msg = (SocketUserMessage) message;
            var context = new SocketCommandContext(Client.SocketClient, msg);
            var serviceProvider = await Client.GetProviderAsync(context.Guild.Id);

            //pos will be the place we're at in the message after we check for the command prefix
            var pos = 0;
            var prefix = serviceProvider.GetService<SettingsService>()
                .Prefix;

            //check if the message is null/etc, checks if it's command prefixed or user mentioned
            if (!string.IsNullOrWhiteSpace(context.Message?.Content) && !context.User.IsBot &&
                (msg.HasStringPrefix(prefix, ref pos) || msg.HasMentionPrefix(Client.SocketClient.CurrentUser, ref pos)))
                try
                {
                    //if it is, try to execute the command using serviceprovider
                    await CommandService.ExecuteAsync(context, pos, serviceProvider, MultiMatchHandling.Best);
                } catch (Exception ex)
                {
                    //exceptions shouldnt reach this far, but just in case
                    Log.Error(
                        $"{Environment.NewLine}{Environment.NewLine}UNKNOWN EXCEPTION - SEVERE{Environment.NewLine}{ex.Message}{Environment.NewLine}{Environment.NewLine}");
                }
        }
    }
}