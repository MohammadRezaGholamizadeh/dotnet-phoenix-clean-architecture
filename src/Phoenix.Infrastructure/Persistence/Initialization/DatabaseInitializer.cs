using FluentMigrator;
using Microsoft.Extensions.Logging;
using Phoenix.Domain.MigrationsDomain;
using Phoenix.Infrastructure.Multitenancy;
using Phoenix.Migrations;

namespace Phoenix.Infrastructure.Persistence.Initialization;

internal class DatabaseInitializer : IDatabaseInitializer
{
    private readonly EFDataContext _context;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(
        EFDataContext context,
        ILogger<DatabaseInitializer> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task InitializeDatabasesAsync()
    {
        var lastMigrationVersion =
            typeof(MigrationRunner).Assembly.GetTypes()
            .Where(_ => _.BaseType == typeof(Migration))
            .SelectMany(_ => _.CustomAttributes)
            .Where(_ => _.AttributeType == typeof(MigrationAttribute))
            .Select(_ => _.GetType().GetProperty("Version").GetValue(_))
            .OrderByDescending(_ => _).First();
       


        if (!_context.Set<MigrationVersionInfo>().Any()
            || _context.Set<MigrationVersionInfo>().OrderByDescending(_ => _.Version).First().Version
             < (long)lastMigrationVersion)
        {
            _logger.LogInformation("Applying Root Migrations.");
            MigrationRunner.Run(new string[] { });
        }
    }
}
