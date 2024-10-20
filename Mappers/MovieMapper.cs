using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Responses.Movie;

namespace MovieApi.Mappers
{
    public class MovieMapper
    {
        private readonly AppDbContext _context;

        public MovieMapper(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MovieResponse> ToDto(Movie movie)
        {
            var movieResponse = new MovieResponse
            {
                Id = movie.Id,
                Title = movie.Title,
                Duration = movie.Duration,
                Description = movie.Description,
                IsPublished = movie.IsPublished ? 
                    "Published" : "Unpublished",
                CreatedAt = movie.CreatedAt?.ToString("dd MMM yyyy HH:mm:ss"),
                UpdatedAt = movie.UpdatedAt?.ToString("dd MMM yyyy HH:mm:ss"),

            };

            var listGenres = await _context.MovieGenres
                .Where(mg => mg.MovieId == movie.Id && 
                    mg.Deleted == false)
                .Join(_context.Genres, 
                        mg => mg.GenreId, 
                        g => g.Id, 
                        (mg, g) => g.Name)
                .ToListAsync();

            if (listGenres != null)
            {
                movieResponse.ListOfGenres = listGenres;
            }

            return movieResponse;
        }

        public async Task<IEnumerable<MovieResponse>> ToDtos(List<Movie> movies)
        {
            return await Task.Run(() =>
            {
                var listMovieResponse = new List<MovieResponse>();
                foreach (var movie in movies)
                {
                    listMovieResponse.Add(ToDto(movie).Result);

                }
                return listMovieResponse;
            });
        }
    }
}