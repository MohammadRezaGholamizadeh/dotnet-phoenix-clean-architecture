using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Phoenix.Application.Common.Tokens;
using Phoenix.Domain.Entities.ApplicationUsers;

namespace Phoenix.DataSources.Infrastructures.DBContexts
{
    public class IdentityDataContext :
        IdentityDbContext<ApplicationUser
                          , ApplicationRole
                          , string
                          , ApplicationUserClaim
                          , ApplicationUserRole
                          , ApplicationUserLogin
                          , ApplicationRoleClaim
                          , ApplicationUserToken>
    {
        private const string TenantName = "TenantId";
        private readonly string _tenantId;

        public IdentityDataContext(
            string connectionString,
            UserTokenService userTokenService)
             : this(new DbContextOptionsBuilder<EFDataContext>()
                    .UseSqlServer(connectionString).Options,userTokenService)
        {
            _tenantId = userTokenService.TenantId;
        }

        protected IdentityDataContext(
            DbContextOptions dbContextOptions,
            UserTokenService userTokenService)
                  : base(dbContextOptions)
        {
            _tenantId = userTokenService.TenantId;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public override ChangeTracker ChangeTracker
        {
            get
            {
                var tracker = base.ChangeTracker;
                tracker.LazyLoadingEnabled = false;
                tracker.AutoDetectChangesEnabled = true;
                tracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                return tracker;
            }
        }
        public async Task<int> SaveChangesAsync()
        {
            foreach (var entityEntry in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added))
            {
                var pp = entityEntry.Metadata.FindProperty(TenantName);

                if (pp != null &&
                    entityEntry.Property(TenantName).CurrentValue == null)
                    entityEntry.Property(TenantName).CurrentValue = _tenantId;
            }
            return await base.SaveChangesAsync();
        }
        public override int SaveChanges()
        {
            foreach (var entityEntry in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added))
            {
                var pp = entityEntry.Metadata.FindProperty(TenantName);

                if (pp != null &&
                    entityEntry.Property(TenantName).CurrentValue == null)
                    entityEntry.Property(TenantName).CurrentValue = _tenantId;
            }
            return base.SaveChanges();
        }
        public override int SaveChanges(bool acceptAllWhenSucccess)
        {
            foreach (var entityEntry in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added))
            {
                var pp = entityEntry.Metadata.FindProperty(TenantName);

                if (pp != null &&
                    entityEntry.Property(TenantName).CurrentValue == null)
                    entityEntry.Property(TenantName).CurrentValue = _tenantId;
            }
            return base.SaveChanges(acceptAllWhenSucccess);
        }
    }
}
