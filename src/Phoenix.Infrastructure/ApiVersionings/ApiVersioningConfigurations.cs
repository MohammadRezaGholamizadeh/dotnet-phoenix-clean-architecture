using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Infrastructure.ApiVersionings;
using Serilog;

namespace Phoenix.Infrastructure.ApiVersionings
{
    public static class ApiVersioningConfigurations
    {
        public static IServiceCollection AddApiVersioning(
        this IServiceCollection services,
        IConfiguration apiConfig)
        {
            var apiVersion =
                apiConfig.GetSection(nameof(ApiVersioning).ToLower())
                         .Get<ApiVersioning>();
            Log.Information(
                $"Default Api Version : " +
                   $"{apiVersion.MajorApiVersion}" +
                   $".{apiVersion.MinorApiVersion}");

            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            return services.AddApiVersioning(
                   config =>
                   {
                       config.DefaultApiVersion =
                              new ApiVersion(
                                  apiVersion.MajorApiVersion,
                                  apiVersion.MinorApiVersion);
                       config.AssumeDefaultVersionWhenUnspecified =
                              apiVersion.AssumeDefaultVersionWhenUnspecified;
                       config.ReportApiVersions =
                              apiVersion.ReportApiVersions;
                   });
        }
    }
}
