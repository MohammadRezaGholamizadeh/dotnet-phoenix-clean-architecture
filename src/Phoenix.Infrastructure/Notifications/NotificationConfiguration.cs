using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Phoenix.Infrastructure.Notifications;

internal static class NotificationConfiguration
{
    internal static IServiceCollection AddNotifications(
        this IServiceCollection services,
        IConfiguration config)
    {
        ILogger logger =
           Log.ForContext(typeof(NotificationConfiguration));

        var signalRSettings =
            config.GetSection(nameof(SignalRConfigurations))
                  .Get<SignalRConfigurations>();

        if (!signalRSettings.UseBackPlane)
        {
            services.AddSignalR();
        }
        else
        {
            var backPlaneSettings =
                config.GetSection("SignalRConfigurations")
                      .GetSection("BackPlane")
                      .Get<SignalRConfigurations.BackPlane>();

            if (backPlaneSettings is null)
                throw new InvalidOperationException(
                    "BackPlane Enabled, But No BackPlane Configurations In Config.");

            switch (backPlaneSettings.Provider)
            {
                case "redis":
                    if (backPlaneSettings.StringConnection is null)
                        throw new InvalidOperationException(
                            "Redis BackPlane Provider:" +
                            " No ConnectionString Configured.");
                    services.AddSignalR()
                            .AddStackExchangeRedis(
                                backPlaneSettings.StringConnection,
                                options =>
                                {
                                    options.Configuration
                                           .AbortOnConnectFail = false;
                                });
                    break;

                default:
                    throw new InvalidOperationException(
                        $"SignalR BackPlane Provider {backPlaneSettings.Provider} " +
                        $"Is Not Supported.");
            }

            logger.Information(
                $"SignalR BackPlane Current Provider: " +
                $"{backPlaneSettings.Provider}.");
        }

        return services;
    }

    internal static IEndpointRouteBuilder MapNotifications(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<NotificationHub>("/notifications", options =>
        {
            options.CloseOnAuthenticationExpiration = true;
        });
        return endpoints;
    }
}
