using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Phoenix.Application.Infrastructures.TokenManagements.Contracts;
using System.Text;

namespace Phoenix.Infrastructure.Auth.Jwt;

internal static class JwtConfiguration
{
    internal static IServiceCollection
        AddJwtAuth(
           this IServiceCollection services,
           IConfiguration config)
    {
        var jwtConfig = new JwtBearerTokenConfig();
        config.GetSection(nameof(JwtBearerTokenConfig)).Bind(jwtConfig);
        services.Configure<JwtBearerTokenConfig>(config.GetSection(nameof(JwtBearerTokenConfig)));
        services.AddOptions<JwtSettings>()
                .BindConfiguration($"SecurityConfiguration:{nameof(JwtSettings)}")
                .ValidateDataAnnotations()
                .ValidateOnStart();
        var key = Encoding.ASCII.GetBytes(jwtConfig.SecretKey);
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
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}
