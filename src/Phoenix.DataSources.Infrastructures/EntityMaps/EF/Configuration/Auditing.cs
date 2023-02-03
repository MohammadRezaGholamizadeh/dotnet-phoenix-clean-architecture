using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Phoenix.SharedConfiguration.Audit;

namespace Phoenix.DataSources.Infrastructures.EntityMaps.EF.Configuration
{
    public class AuditTrailConfig : IEntityTypeConfiguration<Trail>
    {
        public void Configure(EntityTypeBuilder<Trail> builder) =>
            builder
                .ToTable("AuditTrails", SchemaNames.Auditing);
    }
}