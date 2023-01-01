using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Phoenix.Infrastructure.Middleware;

internal static class ExceptionMiddleWareConfiguration
{
    internal static IServiceCollection AddExceptionMiddleware(
        this IServiceCollection services)
    {
        return services.AddScoped<ExceptionMiddleware>();
    }

    internal static IApplicationBuilder UseExceptionMiddleware(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }

    internal static IServiceCollection AddRequestLogging(
        this IServiceCollection services,
        IConfiguration config)
    {
        if (GetMiddlewareConfigurations(config).EnableHttpsLogging)
        {
            services.AddSingleton<RequestLoggingMiddleware>();
            services.AddScoped<ResponseLoggingMiddleware>();
        }

        return services;
    }

    internal static IApplicationBuilder UseRequestLogging(
        this IApplicationBuilder app,
        IConfiguration config)
    {
        if (GetMiddlewareConfigurations(config).EnableHttpsLogging)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ResponseLoggingMiddleware>();
        }

        return app;
    }

    private static MiddlewareConfiguration GetMiddlewareConfigurations(
        IConfiguration config)
    {
        return config.GetSection(nameof(MiddlewareConfiguration))
                     .Get<MiddlewareConfiguration>();
    }

}
