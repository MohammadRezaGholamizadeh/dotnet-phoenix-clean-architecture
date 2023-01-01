using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Phoenix.Domain.MigrationsDomain;

namespace Phoenix.Persistance.EF.EntityMaps.MigrationsDomain
{
    public class EFMigrationVersionInfoEntityMap
        : IEntityTypeConfiguration<MigrationVersionInfo>
    {
        public void Configure(EntityTypeBuilder<MigrationVersionInfo> _)
        {
            _.ToTable("VersionInfo");
            _.HasNoKey();

            _.Property(_ => _.Version).IsRequired();
            _.Property(_ => _.Description).IsRequired();
            _.Property(_ => _.AppliedOn).IsRequired();
        }
    }
}
