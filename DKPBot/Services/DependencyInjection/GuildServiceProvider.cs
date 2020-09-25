using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace DKPBot.Services.DependencyInjection
{
    internal class GuildServiceProvider : IServiceProvider
    {
        private readonly IServiceCollection Services;

        internal GuildServiceProvider()
            : this(new ServiceCollection()) { }

        internal GuildServiceProvider(IServiceCollection services) => Services = services;

        public object GetService(Type serviceType) => Services.First(service => service.ServiceType == serviceType).ImplementationInstance;
    }
}