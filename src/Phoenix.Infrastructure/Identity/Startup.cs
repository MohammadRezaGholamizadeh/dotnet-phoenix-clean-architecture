using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.DataSources.Infrastructures.DBContexts;
using Phoenix.SharedConfiguration.Common.ApplicationUsers;

namespace Phoenix.Infrastructure.Identity;

internal static class Startup
{
    internal static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        return services
            .AddIdentity<ApplicationUser, ApplicationRoles>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<EFDataContext>()
            .AddDefaultTokenProviders()
            .Services;
    }
}
