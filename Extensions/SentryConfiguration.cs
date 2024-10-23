namespace MovieApi.Extensions
{
    public static class SentryConfiguration
    {
        public static void ConfigureSentry(this WebApplicationBuilder builder)
        {
            builder.WebHost.UseSentry(options =>
            {
                options.Dsn = builder.Configuration["Sentry:Dsn"];
                options.Debug = true;
                options.TracesSampleRate = 1.0;
                options.Environment = builder.Environment.EnvironmentName;
                options.AttachStacktrace = true;
            });
        }        
    }
}