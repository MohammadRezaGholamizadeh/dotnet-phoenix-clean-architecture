using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using ZymLabs.NSwag.FluentValidation;

namespace Phoenix.Infrastructure.OpenApi;

internal static class OpenApiConfiguration
{
    internal static IServiceCollection AddOpenApiDocumentation(
        this IServiceCollection services,
        IConfiguration config)
    {
        var settings =
            config.GetSection(nameof(SwaggerConfigurations))
                  .Get<SwaggerConfigurations>();
        if (settings.Enable)
        {
            services.AddVersionedApiExplorer(
                o => o.SubstituteApiVersionInUrl = true);
            services.AddEndpointsApiExplorer();

            services.AddRouting();

            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(_ => _.FullName);
                options.OperationFilter<SwaggerHeaderParameter>();
                var versionProvider =
                    services
                    .BuildServiceProvider()
                    .GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var _ in versionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(_.GroupName, CreateApiInformation(_));
                }

                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Example: \"Bearer token\""
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddScoped<FluentValidationSchemaProcessor>();
        }

        return services;
    }

    internal static IApplicationBuilder UseOpenApiDocumentation(
        this IApplicationBuilder app,
        IConfiguration config)
    {
        if (config.GetSection("SwaggerConfigurations")
                  .GetValue<bool>("Enable"))
        {
            var provider =
                app.ApplicationServices
                .GetRequiredService<IApiVersionDescriptionProvider>();
            var environment =
                app.ApplicationServices
                .GetRequiredService<IHostEnvironment>();

            if (environment.IsDevelopment())
            {
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "def/{documentName}/swagger.json";
                });

                app.UseSwaggerUI(options =>
                {
                    options.RoutePrefix = "def";

                    foreach (var _ in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                           $"{_.GroupName}/swagger.json",
                           _.GroupName.ToUpperInvariant());
                    }
                    options.DocumentTitle = $"Phoenix Clean Architecture API Documentation";

                    if (config.GetSection("SecurityConfiguration")
                          .GetValue<string>("Provider")
                          .Equals("AzureAd", StringComparison.OrdinalIgnoreCase))
                    {
                        options.OAuthConfigObject = new OAuthConfigObject()
                        {
                            AppName = "Phoenix Api Client",
                            ClientId =
                               config.GetSection("SecurityConfiguration")
                                     .GetSection("Swagger")
                                     .GetValue<string>("OpenIdClientId"),
                            ClientSecret = string.Empty,
                            UsePkceWithAuthorizationCodeGrant = true,
                            ScopeSeparator = " "
                        };
                        options.OAuthConfigObject.Scopes
                               .Append(config.GetSection("SecurityConfiguration")
                               .GetSection("Swagger")
                               .GetValue<string>("ApiScope"));
                    }

                });
            }
        }

        return app;
    }


    static OpenApiInfo CreateApiInformation(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = $"Phoenix Clean Architecture API {description.ApiVersion}",
            Version = description.ApiVersion.ToString(),
            Description = "API Documentation.",
            Contact = new OpenApiContact
            {
                Name = "Phoenix Team",
                Email = "masteroffire2000@gmail.com"
            },
            License = new OpenApiLicense
            {
                Name = "Phoenix Team",
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}
