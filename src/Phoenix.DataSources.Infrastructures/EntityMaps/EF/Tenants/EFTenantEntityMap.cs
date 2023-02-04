using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Phoenix.Domain.Entities.Tenants;

namespace Phoenix.DataSources.Infrastructures.EntityMaps.EF.Tenants
{
    public class EFTenantEntityMap : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> _)
        {
            _.ToTable("Tenants");
            _.HasKey(_ => _.Id);

            _.Property(_ => _.Name).IsRequired();
            _.Property(_ => _.Email).IsRequired();
            _.OwnsOne(_ => _.Mobile,
                      _ =>
                      {
                          _.Property(_ => _.CountryCallingCode)
                           .HasColumnName("Mobile_CountryCallingCode")
                           .IsRequired();
                          _.Property(_ => _.MobileNumber)
                           .HasColumnName("Mobile_Number")
                           .IsRequired();
                      });
            _.Property(_ => _.IsActive).IsRequired();

            _.HasMany(_ => _.Users)
                .WithOne(_ => _.Tenant)
                .HasForeignKey(_ => _.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
