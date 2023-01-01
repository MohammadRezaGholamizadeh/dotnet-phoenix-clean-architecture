namespace Phoenix.RestApi.Configurations;

internal static class Configurations
{
    internal static ConfigureHostBuilder AddConfigurations(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((context, config) =>
        {
            var baseDirectory = Directory.GetCurrentDirectory()+ @"\Configurations";

            var s = config.SetBasePath(baseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("cacheconfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("corsconfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("databaseconfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("hangfireconfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("localizationconfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("loggerconfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("mailconfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("middlewareconfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("openapiconfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("securityconfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("securityheadersconfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("signalrconfig.json", optional: true, reloadOnChange: true)
            .AddJsonFile("apiversioningconfig.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        });
        return host;
    }
}
