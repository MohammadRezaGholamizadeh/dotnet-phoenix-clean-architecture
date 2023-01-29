using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Phoenix.Migrations;

namespace Phoenix.Infrastructure.Persistence.Initialization;

internal class DatabaseInitializer : IDatabaseInitializer
{
    private readonly ILogger<DatabaseInitializer> _logger;
    private readonly IConfiguration _config;

    public DatabaseInitializer(
        ILogger<DatabaseInitializer> logger,
        IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }
    public void InitializeDatabasesAsync()
    {
        _logger.LogInformation("Applying Root Migrations ...");
        var connectionString =
            _config
            .GetSection("PersistenceConfig")
            .GetValue<string>("ConnectionString");
        MigrationRunner.CreateDatabaseSchema(connectionString);
        Thread.Sleep(3000);
        MigrationRunner.RunRootMigrations(new string[] { });
        _logger.LogInformation(
            Environment.NewLine +
            "*******  | DataBase Migration Updated Successfully | *******" +
            Environment.NewLine);
    }
}
