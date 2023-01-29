using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Phoenix.Domain.Entities.Colors;

namespace Phoenix.DataSources.Infrastructures.EntityMaps.EF.Colors
{
    public class EFColorEntityMap : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> _)
        {
            _.ToTable("Colors");
            _.HasKey(_ => _.Id);

            _.Property(_ => _.Title).IsRequired();
        }
    }
}
