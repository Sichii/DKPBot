using System;
using Microsoft.Extensions.DependencyInjection;

namespace DKPBot.Services.DependencyInjection
{
    internal class GuildServiceBuilder
    {
        private ulong GuildId;
        internal IServiceCollection Services { get; set; }

        internal GuildServiceBuilder(IServiceCollection services) => Services = services;

        internal GuildServiceProvider Build()
        {
            if (GuildId == 0)
                throw new InvalidOperationException("No guild id provided");

            return new GuildServiceProvider(Services);
        }

        internal void Configure(ulong guildId) => GuildId = guildId;
    }
}