using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace DKPBot.Services.DependencyInjection
{
    internal class GuildServiceBuilder
    {
        private readonly IServiceCollection Services;

        internal GuildServiceBuilder(IServiceCollection serviceCollection) => Services = serviceCollection;

        internal void Configure(ulong guildId)
        {
            var typeObj = typeof(GuildServiceBase);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(assmType => typeObj.IsAssignableFrom(assmType) && assmType != typeObj);

            foreach (var type in types)
            {
                var service = Activator.CreateInstance(type, guildId);
                Services.Add(new ServiceDescriptor(type, service));
            }
        }

        internal GuildServiceProvider BuildAsync()
        {
            if (Services.Count == 0)
                throw new InvalidOperationException("No services located. Please configure with guild id.");

            return new GuildServiceProvider(Services);
        }
    }
}