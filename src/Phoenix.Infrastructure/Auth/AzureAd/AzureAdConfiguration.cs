using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Serilog;

namespace Phoenix.Infrastructure.Auth.AzureAd;

internal static class AzureAdConfiguration
{
    internal static IServiceCollection AddAzureAdAuth(
        this IServiceCollection services,
        IConfiguration config)
    {
        var logger =
            Log.ForContext(typeof(AzureAdJwtBearerEvents));

        services.AddAuthorization()
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = 
                        JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = 
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddMicrosoftIdentityWebApi(
                    jwtOptions => 
                        jwtOptions.Events = 
                           new AzureAdJwtBearerEvents(logger, config),
                    msIdentityOptions => 
                        config.GetSection("SecurityConfiguration")
                              .GetSection("AzureAd")
                              .Bind(msIdentityOptions));

        return services;
    }
}
