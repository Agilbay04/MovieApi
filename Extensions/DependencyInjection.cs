using MovieApi.Mappers;
using MovieApi.Services.AuthService;
using MovieApi.Services.BookingService;
using MovieApi.Services.GenreService;
using MovieApi.Services.MovieService;
using MovieApi.Services.PriceService;
using MovieApi.Services.RoleService;
using MovieApi.Services.ShowtimeService;
using MovieApi.Services.StudioService;
using MovieApi.Services.UploadService;
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
            services.AddScoped<IStudioService, StudioService>();
            services.AddScoped<StudioMapper>();
            services.AddScoped<DateUtil>();
            services.AddScoped<SeatMapper>();
            services.AddScoped<IPriceService, PriceService>();
            services.AddScoped<PriceMapper>();
            services.AddScoped<PriceUtil>();
            services.AddScoped<IShowtimeService, ShowtimeService>();
            services.AddScoped<ShowtimeMapper>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<BookingMapper>();
            services.AddScoped<CodeUtil>();
        }
    }
}