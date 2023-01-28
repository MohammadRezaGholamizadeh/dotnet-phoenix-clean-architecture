using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Phoenix.Infrastructure.RoutingConfig
{
    public static class ServerConfiguration
    {
        public static void AddServerConfiguration(
            this WebApplicationBuilder webApplicationBuilder,
            IConfiguration configuration)
        {
            var url = configuration.GetValue<string>("ServerBootUrl");
            webApplicationBuilder.WebHost
                .UseUrls(url);

            webApplicationBuilder.WebHost.UseKestrel();

            webApplicationBuilder.WebHost.UseIISIntegration();
        }
    }
}
