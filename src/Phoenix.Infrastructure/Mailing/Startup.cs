using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Phoenix.Infrastructure.Mailing;

internal static class Startup
{
    internal static IServiceCollection AddMailing(
        this IServiceCollection services, 
        IConfiguration config) 
             => services.Configure<MailConfig>(
                 config.GetSection(nameof(MailConfig)));
}
