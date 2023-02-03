using Microsoft.Extensions.DependencyInjection;
using Phoenix.Application.Infrastructures.TokenManagements.Contracts;
using Phoenix.Infrastructure.Tokens;
using Phoenix.SharedConfiguration.Common.Contracts.Services;

namespace Phoenix.Infrastructure.Common;

internal static class ServiceConfiguration
{
    internal static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddScoped(typeof(JwtBearerTokenConfig));
        services.AddScoped<UserTokenService , UserTokenAppService>();
        var allServices =
            AppDomain.CurrentDomain
                     .GetAssemblies()
                     .SelectMany(_ => _.GetTypes())
                     .Where(_ => !_.IsInterface
                              && _.GetInterfaces()
                                  .Any(_ => typeof(ScopedService).IsAssignableFrom(_)
                                         || typeof(TransientService).IsAssignableFrom(_)
                                         || typeof(SingletonService).IsAssignableFrom(_)))
                     .Select(service => new
                     {
                         RepositoryType = service,
                         InterfaceType = service.GetInterfaces()
                                 .Where(_ => typeof(ScopedService).IsAssignableFrom(_)
                                          || typeof(TransientService).IsAssignableFrom(_)
                                          || typeof(SingletonService).IsAssignableFrom(_))
                                 .ToList()
                     });
        if (allServices.Any(_ => _.InterfaceType.Count > 2))
        {
            throw new InvalidOperationException(
                "Each Service Can Have Just One LifeTime And Same As Its Service LifeTime !!!");
        }
        foreach (var service in allServices)
        {
            if (service.InterfaceType.Any(_ => _ == typeof(ScopedService)))
            {
                services.AddScoped(
                         service.InterfaceType.Single(_ => _ != typeof(ScopedService)),
                         service.RepositoryType);
            }

            else if (service.InterfaceType.Any(_ => _ == typeof(TransientService)))
            {
                services.AddTransient(
                         service.InterfaceType.Single(_ => _ != typeof(TransientService)),
                         service.RepositoryType);
            }
            else if (service.InterfaceType.Any(_ => _ == typeof(SingletonService)))
            {
                services.AddSingleton(
                         service.InterfaceType.Single(_ => _ != typeof(SingletonService)),
                         service.RepositoryType);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Can Not Inject Type: {service.RepositoryType} Successfully !!!");
            }
        }

        return services;
    }
}
