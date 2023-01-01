using Autofac;
using Autofac.Extensions.DependencyInjection;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentValidation.AspNetCore;
using Phoenix.Infrastructure;
using Phoenix.Infrastructure.Common;
using Phoenix.RestApi;
using Phoenix.RestApi.Configurations;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

[assembly: ApiConventionType(typeof(PhoenixApiConventions))]

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.AddConfigurations();
    builder.Host.UseSerilog((_, config) =>
    {
        config.WriteTo.Console(
               theme: AnsiConsoleTheme.Literate)
        .ReadFrom.Configuration(builder.Configuration);
    });
    builder.WebHost.ConfigureServices(_ => _.AddAutofac());
    builder.Services.AddControllers().AddFluentValidation();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    var app = builder.Build();

    //    await app.Services.InitializeDatabasesAsync();

    app.UseInfrastructure(builder.Configuration);
    app.MapEndpoints();
    app.Run();


}

catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}

void ConfigureServices(IServiceCollection services)
{
    services.AddOptions();
    var builder = new ContainerBuilder();
    builder.Populate(services);
    var autofacContainer = builder.Build();

}
