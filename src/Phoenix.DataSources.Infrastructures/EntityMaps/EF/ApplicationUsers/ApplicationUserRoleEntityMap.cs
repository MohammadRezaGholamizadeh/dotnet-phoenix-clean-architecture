using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Phoenix.Domain.Entities.ApplicationUsers;

namespace Phoenix.DataSources.Infrastructures.EntityMaps.EF.ApplicationUsers
{
    public class ApplicationUserRoleEntityMap
        : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> _)
        {
            _.ToTable("ApplicationUserRoles");
            _.HasKey("UserId", "RoleId");
        }
    }
}
