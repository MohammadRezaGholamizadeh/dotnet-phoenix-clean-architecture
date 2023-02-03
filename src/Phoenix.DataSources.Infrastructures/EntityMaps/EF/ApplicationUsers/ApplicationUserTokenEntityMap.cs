using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Phoenix.Domain.Entities.ApplicationUsers;

namespace Phoenix.DataSources.Infrastructures.EntityMaps.EF.ApplicationUsers
{
    public class ApplicationUserTokenEntityMap
        : IEntityTypeConfiguration<ApplicationUserToken>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserToken> _)
        {
            _.ToTable("ApplicationUserTokens");
            _.HasKey("UserId", "LoginProvider", "Name");
        }
    }
}
