using Microsoft.Extensions.Configuration;

namespace Phoenix.Infrastructure
{
    public static class ConfigurationTools
    {
        private static IConfigurationRoot configuration =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                    "appsettings.json",
                    optional: true,
                    reloadOnChange: true)
                .Build();
        public static string GetConnectionString()
        {
            var settings = new AppSettings();
            settings.ConnectionString
                = configuration
                  .GetSection("PersistenceConfig")
                  .GetValue<string>("ConnectionString");
            return settings.ConnectionString;
        }
        public static string GetLogTableName()
        {
            var settings = new AppSettings();
            settings.LogTableName
                = configuration
                  .GetSection("SeriLogConfiguration")
                  .GetValue<string>("LogTableName");
            return settings.LogTableName;
        }
    }
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string LogTableName { get; set; }
    }
}
