using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Application.Common.Interfaces;
using Phoenix.Infrastructure.Auth.AzureAd;
using Phoenix.Infrastructure.Auth.Jwt;
using Phoenix.Infrastructure.Auth.Permissions;
using Phoenix.Infrastructure.Identity;

namespace Phoenix.Infrastructure.Auth;

internal static class Startup
{
    internal static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddCurrentUser()
                .AddPermissionServices()
                .AddIdentity()
                .Configure<SecurityConfiguration>(
                   config.GetSection(nameof(SecurityConfiguration)));

        if (config.GetSection("SecurityConfiguration")
            .GetValue<string>("Provider")
            .Equals("AzureAd", StringComparison.OrdinalIgnoreCase))
        {
            services.AddAzureAdAuth(config);
        }
        else
        {
            services.AddJwtAuth(config);
        }

        return services;
    }

    internal static IApplicationBuilder UseCurrentUser(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<CurrentUserMiddleware>();
    }

    private static IServiceCollection AddCurrentUser(
        this IServiceCollection services)
    {
        return services.AddScoped<CurrentUserMiddleware>()
                       .AddScoped<ICurrentUser, CurrentUser>()
                       .AddScoped(sp =>
                         (ICurrentUserInitializer)sp
                          .GetRequiredService<ICurrentUser>());
    }


    private static IServiceCollection AddPermissionServices(
        this IServiceCollection services)
    {
        return services.AddSingleton<
                           IAuthorizationPolicyProvider,
                           PermissionPolicyProvider>()
                       .AddScoped<
                           IAuthorizationHandler,
                           PermissionAuthorizationHandler>();
    }
}
