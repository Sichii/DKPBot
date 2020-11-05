using System;
using Microsoft.Extensions.DependencyInjection;

namespace DKPBot.Services.DependencyInjection
{
    internal class GuildServiceProviderFactory : IServiceProviderFactory<GuildServiceBuilder>
    {
        public GuildServiceBuilder CreateBuilder(IServiceCollection services) => new GuildServiceBuilder(services);

        public IServiceProvider CreateServiceProvider(GuildServiceBuilder containerBuilder) => containerBuilder.BuildAsync();
    }
}