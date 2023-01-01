using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Phoenix.Infrastructure.Auth.Jwt;

internal static class JwtConfiguration
{
    internal static IServiceCollection
        AddJwtAuth(
           this IServiceCollection services,
           IConfiguration config)
    {
        services.AddOptions<JwtSettings>()
                .BindConfiguration($"SecurityConfiguration:{nameof(JwtSettings)}")
                .ValidateDataAnnotations()
                .ValidateOnStart();

        services.AddSingleton<
                    IConfigureOptions<JwtBearerOptions>,
                    ConfigureJwtBearerOptions>()
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, null!);

        return services;
    }
}
