using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DKPBot.Definitions;
using DKPBot.Discord;
using DKPBot.Services;
using DKPBot.Services.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace DKPBot.Model
{
    internal static class Client
    {
        internal static readonly DiscordSocketClient SocketClient;
        private static readonly string Token;
        private static readonly Logger Log;
        private static readonly GuildServiceProviderFactory Factory;
        private static readonly IDictionary<ulong, IServiceProvider> Providers;

        static Client()
        {
            Token = File.ReadAllText(CONSTANTS.TOKEN_PATH);
            Providers = new Dictionary<ulong, IServiceProvider>();
            Log = LogManager.GetLogger("Client");
            Factory = new GuildServiceProviderFactory();

            var config = new DiscordSocketConfig { LogLevel = LogSeverity.Debug };

            SocketClient = new DiscordSocketClient(config);
            SocketClient.Log += LogMessage;
            SocketClient.MessageReceived += CommandHandler.TryHandleAsync;
            SocketClient.Ready += () =>
                SocketClient.SetActivityAsync(new Activity("hard to get", ActivityType.Playing, ActivityProperties.None, string.Empty));
        }

        internal static async Task LoginAsync()
        {
            var defaultProvider = await CreateProviderAsync(ulong.MaxValue);
            await CommandHandler.ConfigureAsync(defaultProvider);
            await SocketClient.LoginAsync(TokenType.Bot, Token);
            await SocketClient.StartAsync();
        }

        /// <summary>
        ///     Interceptor for logging messages from Discord.NET
        /// </summary>
        /// <param name="msg">A discord log message</param>
        /// <returns></returns>
        private static Task LogMessage(LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Log.Error(msg.Message);
                    break;
                case LogSeverity.Warning:
                    Log.Warn(msg.Message);
                    break;
                case LogSeverity.Info:
                    Log.Info(msg.Message);
                    break;
                case LogSeverity.Verbose:
                    Log.Trace(msg.Message);
                    break;
                case LogSeverity.Debug:
                    Log.Debug(msg.Message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(msg.Severity), msg.Severity, null);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Gets or creates a service provider for a guild.
        /// </summary>
        /// <param name="guildId">the ID of a SocketGuild</param>
        internal static async Task<IServiceProvider> CreateProviderAsync(ulong guildId)
        {
            //if we dont have a serviceprovider for this guild
            if (!Providers.TryGetValue(guildId, out var serviceProvider))
            {
                var services = new ServiceCollection();

                Log.Debug($"Building new service provider for guild {guildId}...");
                var builder = Factory.CreateBuilder(services);

                Log.Debug($"Configuring services for guild {guildId}...");
                builder.Configure(guildId);

                serviceProvider = Factory.CreateServiceProvider(builder);
                Providers[guildId] = serviceProvider;

                Log.Debug($"Populating serializable services for guild {guildId}...");
                foreach (var service in services.Select(service => service.ImplementationInstance)
                    .OfType<ISerializableGuildService>())
                    await service.PopulateAsync();
            }

            return serviceProvider;
        }
    }
}