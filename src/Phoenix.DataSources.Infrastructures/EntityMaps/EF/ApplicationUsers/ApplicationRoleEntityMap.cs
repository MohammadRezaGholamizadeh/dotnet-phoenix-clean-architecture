using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Phoenix.Domain.Entities.ApplicationUsers;

namespace Phoenix.DataSources.Infrastructures.EntityMaps.EF.ApplicationUsers
{
    public class ApplicationRoleEntityMap
        : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> _)
        {
            _.ToTable("ApplicationRoles");
            _.HasKey(_ => _.Id);
        }
    }
}
