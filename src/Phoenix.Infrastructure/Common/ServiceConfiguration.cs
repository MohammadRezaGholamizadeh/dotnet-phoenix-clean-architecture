using Microsoft.Extensions.DependencyInjection;
using Phoenix.Application.Common.Interfaces;

namespace Phoenix.Infrastructure.Common;

internal static class ServiceConfiguration
{
    internal static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        return services.AddServices(
                            typeof(ITransientService),
                            ServiceLifetime.Transient)
                       .AddServices(
                            typeof(IScopedService),
                            ServiceLifetime.Scoped);
    }


    internal static IServiceCollection AddServices(
        this IServiceCollection services,
        Type interfaceType,
        ServiceLifetime lifetime)
    {
        var interfaceTypes =
            AppDomain.CurrentDomain
                     .GetAssemblies()
                     .SelectMany(assembly => assembly.GetTypes())
                     .Where(type => interfaceType.IsAssignableFrom(type)
                                 && type.IsClass && !type.IsAbstract)
                     .Select(type => new
                     {
                         Service =
                            type.GetInterfaces().FirstOrDefault(),
                         Implementation = type
                     })
                     .Where(_ => _.Service is not null
                              && interfaceType.IsAssignableFrom(_.Service));

        foreach (var type in interfaceTypes)
        {
            services.AddService(type.Service!, type.Implementation, lifetime);
        }

        return services;
    }

    internal static IServiceCollection AddService(
        this IServiceCollection services,
        Type serviceType,
        Type implementationType,
        ServiceLifetime lifetime)
    {
        return lifetime switch
        {
            ServiceLifetime.Transient => 
                services.AddTransient(serviceType, implementationType),
            ServiceLifetime.Scoped => 
                services.AddScoped(serviceType, implementationType),
            ServiceLifetime.Singleton => 
                services.AddSingleton(serviceType, implementationType),
            _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
        };
    }
}
