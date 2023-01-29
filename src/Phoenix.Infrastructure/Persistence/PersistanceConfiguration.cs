using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Phoenix.Application.Common.Persistence;
using Phoenix.DataSources.Infrastructures.DBContexts;
using Phoenix.Infrastructure.Common;
using Phoenix.Infrastructure.Persistence.ConnectionString;
using Phoenix.Infrastructure.Persistence.Initialization;
using Phoenix.Persistance.EF.Repositories;
using Phoenix.SharedConfiguration.Common.Contracts.Repositories;
using Serilog;

namespace Phoenix.Infrastructure.Persistence;

internal static class PersistanceConfiguration
{
    private static readonly ILogger _logger
        = Log.ForContext(typeof(PersistanceConfiguration));
    internal static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration config)
    {
        _logger.Warning(Environment.NewLine + $"===> Validating DB In Progress ..." + Environment.NewLine);
        services
            .AddOptions<DatabaseSettings>()
            .BindConfiguration(nameof(DatabaseSettings))
            .PostConfigure(databaseSettings =>
            {
                _logger
                .Information(
                    Environment.NewLine +
                    $"Application ****** " + Environment.NewLine +
                    $"=> Current DataBase Provider <= " + Environment.NewLine +
                    $"| {'"'}{databaseSettings.DBProvider}{'"'} |" + Environment.NewLine +
                    $"| On DB : {databaseSettings.ConnectionString.Replace(" ", "").Split(";")[1].Replace("database=", "")} |" + Environment.NewLine +
                    $" *** DB Setup Successfully !" + Environment.NewLine);
            })
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Services
            .AddDbContext<EFDataContext>((serviceProvider, options) =>
            {
                var databaseSettings =
                    serviceProvider
                    .GetRequiredService<IOptions<DatabaseSettings>>().Value;
                options.UseDatabase(
                         databaseSettings.DBProvider,
                         databaseSettings.ConnectionString);
            })
            .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
            .AddTransient<CustomSeederRunner>()
            .AddTransient<IConnectionStringSecurer, ConnectionStringSecurer>()
            .AddTransient<IConnectionStringValidator, ConnectionStringValidator>()
            .AddRepositories();
        return services;
    }

    internal static DbContextOptionsBuilder UseDatabase(
        this DbContextOptionsBuilder builder,
        string dbProvider,
        string connectionString)
    {
        switch (dbProvider.ToLowerInvariant())
        {
            case DbProviderKeys.SqlServer:
                return builder.UseSqlServer(connectionString);

            case DbProviderKeys.SqLite:
                return builder.UseSqlite(connectionString);

            default:
                throw new InvalidOperationException(
                $"DataBase Provider {dbProvider} Is Not Supported " +
                $"Or Not Configured Correctly. " +
                $"Check The Configurations That You Put In Json Files");
        }
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var repositories =
            typeof(RepositoryAssemblyBase)
            .Assembly
            .GetExportedTypes()
            .Where(s => s.GetInterfaces()
                         .Any(_ => typeof(ScopedRepository).IsAssignableFrom(_)
                                || typeof(TransientRepository).IsAssignableFrom(_)
                                || typeof(SingletonRepository).IsAssignableFrom(_)))
            .Select(_ => new
            {
                RepositoryType = _,
                InterfaceType = _.GetInterfaces()
                                 .Where(_ => typeof(ScopedRepository).IsAssignableFrom(_)
                                          || typeof(TransientRepository).IsAssignableFrom(_)
                                          || typeof(SingletonRepository).IsAssignableFrom(_))
                                 .ToList()
            });

        if (repositories.Any(_ => _.InterfaceType.Count > 2))
        {
            throw new InvalidOperationException(
                "Each Repository Can Have Just One LifeTime And Same As Its Service LifeTime !!!");
        }
        foreach (var repository in repositories)
        {
            if (repository.InterfaceType.Any(_ => _ == typeof(ScopedRepository)))
            {
                services.AddScoped(
                         repository.InterfaceType.Single(_ => _ != typeof(ScopedRepository)),
                         repository.RepositoryType);
            }

            else if (repository.InterfaceType.Any(_ => _ == typeof(TransientRepository)))
            {
                services.AddTransient(
                         repository.InterfaceType.Single(_ => _ != typeof(TransientRepository)),
                         repository.RepositoryType);
            }
            else if (repository.InterfaceType.Any(_ => _ == typeof(SingletonRepository)))
            {
                services.AddSingleton(
                         repository.InterfaceType.Single(_ => _ != typeof(SingletonRepository)),
                         repository.RepositoryType);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Can Not Inject Type: {repository.RepositoryType} Successfully !!!");
            }
        }

        return services;
    }
}
