using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Seeders;

namespace MovieApi.Extensions
{
    public static class DatabaseConfiguration
    {
        public static void ConfigureDatabase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContextPool<AppDbContext>(options => {
                options.UseLazyLoadingProxies()
                    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();
            await AppDbSeeder.SeedAsync(context);
        }
    }
}