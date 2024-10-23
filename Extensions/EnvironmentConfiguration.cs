namespace MovieApi.Extensions
{
    public static class EnvironmentConfiguration
    {
        public static void ConfigureEnvironment(this WebApplicationBuilder builder)
        {
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}