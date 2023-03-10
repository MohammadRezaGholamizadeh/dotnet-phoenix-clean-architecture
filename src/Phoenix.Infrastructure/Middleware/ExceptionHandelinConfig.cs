using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Phoenix.Infrastructure.Middleware
{
    public class ExceptionHandelinConfig 
    {
        public void ConfigureExceptionHandeling(IApplicationBuilder app)
        {
            var environment =
                app.ApplicationServices
                .GetRequiredService<IWebHostEnvironment>();
            var jsonOptions =
                app.ApplicationServices
                .GetService<IOptions<JsonOptions>>()?
                .Value.JsonSerializerOptions;

            app.UseExceptionHandler(_ => _.Run(async context =>
            {
                var exception =
                  context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
                var errorType =
                  exception?.GetType().Name.Replace("Exception", string.Empty);
                var errorDescription =
                  environment.IsProduction()
                  ? null
                  : exception?.ToString();
                var result = new
                {
                    Error = errorType,
                    Description = errorDescription
                };

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;
                context.Response.ContentType =
                    MediaTypeNames.Application.Json;
                await context.Response
                    .WriteAsync(JsonSerializer.Serialize(result, jsonOptions));
            }));

            if (environment.IsProduction())
            {
                app.UseHsts();
            }
        }
    }
}
