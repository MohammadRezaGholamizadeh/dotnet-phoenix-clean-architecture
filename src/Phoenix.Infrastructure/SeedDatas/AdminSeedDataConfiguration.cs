using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Application.Services.ApplicationUsers.Contracts;
using Phoenix.DataSources.Infrastructures.DBContexts;
using Phoenix.Domain.Entities.ApplicationUsers;
using Phoenix.Domain.Entities.Tenants;

namespace Phoenix.Infrastructure.SeedDatas
{
    public static class AdminSeedDataConfiguration
    {
        public static async Task<IApplicationBuilder> UseSeedData(
            this IApplicationBuilder applicationBuilder,
            IConfiguration configuration)
        {
            var superAdminConfig = new ApplicationUser();
            configuration.GetSection("SuperAdminSeedData").Bind(superAdminConfig);
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
                var applicationUserService =
                scope.ServiceProvider.GetRequiredService<ApplicationUserService>();
                if (!await applicationUserService.IsExistNationalCode(superAdminConfig.NationalCode))
                {
                    var userId =
                        await applicationUserService
                        .AddSuperAdminUserForSeedData(
                            new ApplicationUser()
                            {
                                Id = Guid.NewGuid().ToString(),
                                FirstName = superAdminConfig.FirstName,
                                LastName = superAdminConfig.LastName,
                                UserName = superAdminConfig.UserName,
                                NationalCode = superAdminConfig.NationalCode,
                                Mobile = new Mobile()
                                {
                                    CountryCallingCode =   
                                        superAdminConfig.Mobile.CountryCallingCode,
                                    MobileNumber = 
                                        superAdminConfig.Mobile.MobileNumber,
                                },
                                Tenant = new Tenant()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Email = superAdminConfig.Tenant.Email,
                                    IsActive = true,
                                    Name = superAdminConfig.Tenant.Name,
                                    Mobile = new Mobile()
                                    {
                                        MobileNumber = 
                                            superAdminConfig.Tenant.Mobile.MobileNumber,
                                        CountryCallingCode =   
                                            superAdminConfig.Tenant.Mobile.CountryCallingCode
                                    }
                                }
                            });
                    await applicationUserService.AddUserToAdminRole(userId);
                }
            }
            return applicationBuilder;
        }
    }
}
