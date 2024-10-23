using log4net.Config;

namespace MovieApi.Extensions
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging(this WebApplicationBuilder builder)
        {
            var fileInfo = new FileInfo("log4net.config");
            XmlConfigurator.Configure(fileInfo);

            builder.Logging.ClearProviders();
            builder.Logging.AddLog4Net();
        }
    }
}