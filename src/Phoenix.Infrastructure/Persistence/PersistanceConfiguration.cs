using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Phoenix.Application.Common.Persistence;
using Phoenix.Domain.Common.Contracts;
using Phoenix.Infrastructure.Common;
using Phoenix.Infrastructure.Multitenancy;
using Phoenix.Infrastructure.Persistence.ConnectionString;
using Phoenix.Infrastructure.Persistence.Initialization;
using Phoenix.Infrastructure.Persistence.Repository;
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
            .AddServices(typeof(ICustomSeeder), ServiceLifetime.Transient)
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
        services.AddScoped(typeof(IRepository<>), typeof(ApplicationDbRepository<>));

        foreach (var aggregateRootType in typeof(IAggregateRoot).Assembly
                                          .GetExportedTypes()
                                          .Where(type
                                             => typeof(IAggregateRoot)
                                                .IsAssignableFrom(type)
                                             && type.IsClass)
                                          .ToList())
        {
            services
                .AddScoped(typeof(IReadRepository<>)
                           .MakeGenericType(aggregateRootType),
                           serviceProvider => 
                                serviceProvider.GetRequiredService(typeof(IRepository<>)
                                               .MakeGenericType(aggregateRootType)))
                .AddScoped(typeof(IRepositoryWithEvents<>)
                           .MakeGenericType(aggregateRootType),
                           serviceProvider => 
                                Activator.CreateInstance(typeof(EventAddingRepositoryDecorator<>)
                                         .MakeGenericType(aggregateRootType),
                           serviceProvider.GetRequiredService(typeof(IRepository<>)
                                          .MakeGenericType(aggregateRootType)))
                ?? throw new InvalidOperationException(
                    $"Couldn't create EventAddingRepositoryDecorator " +
                    $"for aggregateRootType {aggregateRootType.Name}"));
        }

        return services;
    }
}
