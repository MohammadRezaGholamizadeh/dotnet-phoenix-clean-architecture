using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Application.Common.Caching;

namespace Phoenix.Infrastructure.Caching;

internal static class CachingConfiguration
{
    internal static IServiceCollection
        AddCaching(
          this IServiceCollection services,
          IConfiguration config)
    {
        var settings =
            config.GetSection(nameof(CacheConfig))
                  .Get<CacheConfig>();

        if (settings.UseDistributedCache)
        {
            if (settings.PreferRedis)
            {
                services.AddStackExchangeRedisCache(
                    options =>
                    {
                        options.Configuration =
                                settings.RedisURL;
                        options.ConfigurationOptions =
                               new StackExchange.Redis
                                   .ConfigurationOptions()
                               {
                                   AbortOnConnectFail = true,
                                   EndPoints = { settings.RedisURL }
                               };
                    });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            services.AddTransient<ICacheService, DistributedCacheService>();
        }
        else
        {
            services.AddMemoryCache();
            services.AddTransient<ICacheService, LocalCacheService>();
        }

        return services;
    }
}
