using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.SQLite;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Infrastructure.BackgroundJobs;
using Phoenix.Infrastructure.Common;
using Serilog;

namespace Phoenix.Infrastructure.BackgroundJobs;

internal static class BackGroundJobConfiguration
{
    private static readonly ILogger _logger =
        Log.ForContext(typeof(BackGroundJobConfiguration));

    internal static IServiceCollection AddBackgroundJobs(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddHangfireServer(
            options => config.GetSection("HangfireConfigurations")
                             .GetSection("Server")
                             .Bind(options));

        services.AddHangfireConsoleExtensions();

        var storageSettings =
            config.GetSection("HangfireConfigurations")
                  .GetSection("Storage")
                  .Get<HangfireStorageSettings>();

        GuardAgainstUnConfiguredStorageProvider(storageSettings);
        GuardAgainstUnConfiguredStorageConnectionString(storageSettings);

        _logger.Information(
            $"Hangfire: Current Storage Provider " +
            $": {storageSettings.StorageProvider}");

        services.AddSingleton<JobActivator, PhoenixJobActivator>()
                .AddHangfire((serviceProvider, hangfireConfig) =>
                {
                    hangfireConfig.UseDatabase(
                            storageSettings.StorageProvider!,
                            storageSettings.ConnectionString!,
                            config)
                         .UseFilter(new PhoenixJobFilter(serviceProvider))
                         .UseFilter(new LogJobFilter())
                         .UseConsole();
                });

        return services;
    }

    private static IGlobalConfiguration UseDatabase(
            this IGlobalConfiguration hangfireConfig,
            string dbProvider,
            string connectionString,
            IConfiguration config)
    {

        if (dbProvider.ToLowerInvariant() == DbProviderKeys.SqlServer)
        {
            return hangfireConfig.UseSqlServerStorage(
                      connectionString,
                      config.GetSection("HangfireConfigurations")
                            .GetSection("Storage")
                            .GetSection("Options")
                            .Get<SqlServerStorageOptions>());
        }
        else if (dbProvider.ToLowerInvariant() == DbProviderKeys.SqLite)
        {
            return hangfireConfig.UseSQLiteStorage(
                       connectionString,
                       config.GetSection("HangfireConfigurations")
                             .GetSection("Storage")
                             .GetSection("Options")
                             .Get<SQLiteStorageOptions>());
        }
        else
        {
            throw new Exception(
                $"Hangfire Storage Provider {dbProvider} is not supported.");
        }

    }


    internal static IApplicationBuilder UseHangfireDashboard(
        this IApplicationBuilder app,
        IConfiguration config)
    {
        var dashboardOptions =
            config.GetSection("HangfireConfigurations")
                  .GetSection("Dashboard")
                  .Get<DashboardOptions>();

        dashboardOptions.Authorization = new[]
        {
           new HangfireCustomBasicAuthenticationFilter
           {
                User =
                  config.GetSection("HangfireConfigurations")
                        .GetSection("Credentials")
                        .GetSection("User")
                        .Value,
                Pass =
                  config.GetSection("HangfireConfigurations")
                        .GetSection("Credentials")
                        .GetSection("Password")
                        .Value
           }
        };

        return app.UseHangfireDashboard(
                   config.GetSection("HangfireConfigurations")
                         .GetValue<string>("Route"),
                   dashboardOptions);
    }

    private static void GuardAgainstUnConfiguredStorageConnectionString(
        HangfireStorageSettings storageSettings)
    {
        if (string.IsNullOrEmpty(storageSettings.ConnectionString))
            throw new Exception(
               "Hangfire Storage Provider ConnectionString is not configured.");
    }

    private static void GuardAgainstUnConfiguredStorageProvider(HangfireStorageSettings storageSettings)
    {
        if (string.IsNullOrEmpty(storageSettings.StorageProvider))
            throw new Exception("Hangfire Storage Provider is not configured.");
    }
}
