using Microsoft.EntityFrameworkCore;
using Phoenix.DataSources.Infrastructures.EntityMaps.EF.MigrationsDomain;
using System.Data;

namespace Phoenix.DataSources.Infrastructures.DBContexts
{
    public class EFDataContext : DbContext
    {
        private string _connectionString;

        public EFDataContext(
            DbContextOptions<EFDataContext> options) : base(options)
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
}