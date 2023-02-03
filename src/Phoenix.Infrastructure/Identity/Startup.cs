using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.DataSources.Infrastructures.DBContexts;
using Phoenix.Domain.Entities.ApplicationUsers;

namespace Phoenix.Infrastructure.Identity;

internal static class Startup
{
    internal static IServiceCollection AddIdentity(
        this IServiceCollection services)
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.AllowedUserNameCharacters = "1234567890";
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = false;
                options.Lockout.AllowedForNewUsers = false;
            })
            .AddEntityFrameworkStores<EFDataContext>()
            .AddDefaultTokenProviders();
        return services;
    }
}
