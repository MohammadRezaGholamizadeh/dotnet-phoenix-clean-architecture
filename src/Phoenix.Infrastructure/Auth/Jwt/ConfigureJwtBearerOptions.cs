using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Phoenix.Application.Common.Exceptions;

namespace Phoenix.Infrastructure.Auth.Jwt;

public class ConfigureJwtBearerOptions
    : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings;

    public ConfigureJwtBearerOptions(
           IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(string.Empty, options);
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        if (name != JwtBearerDefaults.AuthenticationScheme)
        {
            return;
        }

        byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();

                //For Ignoring The Authenticating !

                //if (!context.Response.HasStarted)
                //{
                //    throw new UnauthorizedException("Authentication Failed.");
                //}

                return Task.CompletedTask;
            },
            OnForbidden = _ =>
                throw new ForbiddenException(
                          "You are not authorized to access this resource."),
            OnMessageReceived =
               context =>
               {
                   var accessToken = context.Request.Query["access_token"];
               
                   if (!string.IsNullOrEmpty(accessToken) &&
                       context.HttpContext.Request.Path
                              .StartsWithSegments("/notifications"))
                   {
                       context.Token = accessToken;
                   }
               
                   return Task.CompletedTask;
               }
        };
    }
}
