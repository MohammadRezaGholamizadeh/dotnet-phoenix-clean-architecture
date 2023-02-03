using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Phoenix.Domain.Entities.ApplicationUsers;

namespace Phoenix.DataSources.Infrastructures.EntityMaps.EF.ApplicationUsers
{
    public class ApplicationUserEntityMap
        : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> _)
        {
            _.ToTable("ApplicationUsers");

            _.Property(_ => _.Id).ValueGeneratedNever();
            _.Property(_ => _.FirstName).HasMaxLength(100).IsRequired();
            _.Property(_ => _.LastName).HasMaxLength(100).IsRequired();
            _.Property(_ => _.UserName).HasMaxLength(50);
            _.Property(_ => _.NormalizedUserName).HasMaxLength(50);
            _.Property(_ => _.IsActive).IsRequired();
            _.Property(_ => _.NationalCode)
                .HasMaxLength(10)
                .IsRequired();
            _.OwnsOne(_ => _.Mobile, _ =>
            {
                _.Property(_ => _.CountryCallingCode)
                     .HasMaxLength(5).IsRequired(false)
                     .HasColumnName("Mobile_CountryCallingCode");
                _.Property(_ => _.MobileNumber)
                     .HasMaxLength(11).IsRequired(false)
                     .HasColumnName("Mobile_Number");
            });
            _.Ignore(_ => _.Email)
             .Ignore(_ => _.EmailConfirmed)
             .Ignore(_ => _.NormalizedEmail)
             .Ignore(_ => _.PhoneNumber)
             .Ignore(_ => _.PhoneNumberConfirmed);
        }
    }
}
