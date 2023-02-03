using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Phoenix.Domain.Entities.ApplicationUsers;

namespace Phoenix.DataSources.Infrastructures.EntityMaps.EF.ApplicationUsers
{
    public class ApplicationRoleClaimEntityMap
        : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> _)
        {
            _.ToTable("ApplicationRoleClaims");
            _.HasKey(_ => _.Id);
        }
    }
}
