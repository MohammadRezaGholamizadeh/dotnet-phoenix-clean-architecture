using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public IdentityDataContext(string connectionString)
             : this(new DbContextOptionsBuilder<EFDataContext>()
                    .UseSqlServer(connectionString).Options)
        {
        }

        protected IdentityDataContext(DbContextOptions dbContextOptions)
                  : base(dbContextOptions)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        public override int SaveChanges(bool acceptAllWhenSucccess)
        {
            return base.SaveChanges(acceptAllWhenSucccess);
        }
    }
}
