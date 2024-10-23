namespace MovieApi.Extensions
{
    public static class ControllerConfiguration
    {
        public static void ConfigureControllers(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers().AddJsonOptions(options => 
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            // Configure routes name to lowercase
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
        }
    }
}