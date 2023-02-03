using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Application.Services.ApplicationUsers.Contracts;
using Phoenix.DataSources.Infrastructures.DBContexts;
using Phoenix.Domain.Entities.ApplicationUsers;

namespace Phoenix.Infrastructure.SeedDatas
{
    public static class AdminSeedDataConfiguration
    {
        public static async Task<IApplicationBuilder> UseSeedData(
            this IApplicationBuilder applicationBuilder,
            IConfiguration configuration)
        {
            var roles = new List<string>();
            var config = new ApplicationUser();
            configuration.GetSection("AdminSeedData").Bind(config);
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<EFDataContext>();
                if (!context!.Roles.Any())
                {
                    context!.Add(new ApplicationRole()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = SystemRoles.Admin,
                        NormalizedName = SystemRoles.Admin.ToUpper()
                    });
                    context!.Add(new ApplicationRole()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = SystemRoles.Guest,
                        NormalizedName = SystemRoles.Guest.ToUpper()
                    });
                    await context.SaveChangesAsync();
                }
                var service =
                    scope.ServiceProvider.GetRequiredService<ApplicationUserService>();
                if (!await service.IsExistNationalCcode(config.NationalCode))
                {
                    var userId =
                        await service.AddUser(new AddApplicationUserDto()
                        {
                            FirstName = config.FirstName,
                            LastName = config.LastName,
                            NationalCode = config.NationalCode,
                            CountryCallingCode = config.Mobile.CountryCallingCode,
                            MobileNumber = config.Mobile.MobileNumber
                        });
                    await service.AddUserToAdminRole(userId);
                }
            }
            return applicationBuilder;
        }
    }
}
