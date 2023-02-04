using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Phoenix.DataSources.Infrastructures.DBContexts;

namespace Phoenix.Infrastructure.Persistence
{
    internal static class EFDataContextConfiguration
    {
        internal static IServiceCollection AddEFDataContext(
            this IServiceCollection services,
            IConfiguration config)
        {
            return services
                   .AddDbContext<EFDataContext>(
                (serviceProvider, context) =>
                {
                    var databaseSettings =
                        serviceProvider
                        .GetRequiredService<IOptions<DatabaseSettings>>().Value;
                    context
                      .UseDatabase(
                          databaseSettings.DBProvider,
                          databaseSettings.ConnectionString);
                }, ServiceLifetime.Transient);
        }
    }
}