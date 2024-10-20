using MovieApi.Mappers;
using MovieApi.Services.AuthService;
using MovieApi.Services.GenreService;
using MovieApi.Services.MovieService;
using MovieApi.Services.RoleService;
using MovieApi.Utilities;

namespace MovieApi.Extensions
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<MovieMapper>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<GenreMapper>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<RoleMapper>();
            services.AddScoped<JwtUtil>();
            services.AddScoped<PasswordUtil>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<AuthMapper>();
        }
    }
}