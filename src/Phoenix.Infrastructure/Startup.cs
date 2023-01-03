using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Infrastructure;
using Phoenix.Infrastructure.ApiVersionings;
using Phoenix.Infrastructure.Auth;
using Phoenix.Infrastructure.BackgroundJobs;
using Phoenix.Infrastructure.Caching;
using Phoenix.Infrastructure.Common;
using Phoenix.Infrastructure.Cors;
using Phoenix.Infrastructure.FileStorage;
using Phoenix.Infrastructure.Localization;
using Phoenix.Infrastructure.Mailing;
using Phoenix.Infrastructure.Mapping;
using Phoenix.Infrastructure.Middleware;
using Phoenix.Infrastructure.Multitenancy;
using Phoenix.Infrastructure.Notifications;
using Phoenix.Infrastructure.OpenApi;
using Phoenix.Infrastructure.Persistence;
using Phoenix.Infrastructure.Persistence.Initialization;
using Phoenix.Infrastructure.SecurityHeaders;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace Phoenix.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        MapsterSettings.Configure();
        services.AddHealthChecks();
        return services
            .AddApiVersioning(config)
            .AddAuth(config)
            .AddBackgroundJobs(config)
            .AddCaching(config)
            .AddCorsPolicy(config)
            .AddExceptionMiddleware()
            .AddPOLocalization(config)
            .AddMailing(config)
            .AddEFDataContext(config)
            .AddNotifications(config)
            .AddOpenApiDocumentation(config)
            .AddPersistence(config) // تا اینجا کانفیگ شده
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices();

    }

    public static void InitializeDatabasesAsync(
        this IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
             scope.ServiceProvider
                   .GetRequiredService<IDatabaseInitializer>()
                   .InitializeDatabasesAsync();
        }
    }

    public static IApplicationBuilder UseInfrastructure(
        this IApplicationBuilder builder,
        IConfiguration config)
    {
             builder
            .UseRequestLocalization()
            .UseStaticFiles()
            .UseSecurityHeaders(config)
            .UseFileStorage()
            .UseExceptionMiddleware()
            .UseRouting()
            .UseCorsPolicy()
            .UseAuthentication()
            .UseCurrentUser()
            .UseAuthorization()
            .UseRequestLogging(config)
            .UseHangfireDashboard(config)
            .UseOpenApiDocumentation(config);
        return builder;
    }
        

    public static IEndpointRouteBuilder MapEndpoints(
        this IEndpointRouteBuilder builder)
    {
        builder.MapControllers()
               .RequireAuthorization();
        builder.MapHealthCheck();
        builder.MapNotifications();
        return builder;
    }

    private static IEndpointConventionBuilder MapHealthCheck(
        this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapHealthChecks("/api/health")
                        .RequireAuthorization();

    }
}
