using Microsoft.EntityFrameworkCore;
using Phoenix.Persistance.EF.EntityMaps.MigrationsDomain;
using System.Data;

namespace Phoenix.Infrastructure.Multitenancy;

public class EFDataContext : DbContext
{
    public EFDataContext(DbContextOptions<EFDataContext> options)
        : base(options)
    {
    }

    public EFDataContext(
        string connectionString) : this(
            new DbContextOptionsBuilder<EFDataContext>()
            .UseSqlServer(connectionString).Options)
    {
    }

    public IDbConnection Connection => Database.GetDbConnection();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .ApplyConfigurationsFromAssembly(
             typeof(EFMigrationVersionInfoEntityMap).Assembly);
    }
}
