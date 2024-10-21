using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Responses.Movie;
using MovieApi.Utilities;

namespace MovieApi.Mappers
{
    public class MovieMapper
    {
        private readonly AppDbContext _context;

        private readonly DateUtil _dateUtil;

        public MovieMapper(AppDbContext context, DateUtil dateUtil)
        {
            _context = context;
            _dateUtil = dateUtil;
        }

        public async Task<MovieResponse> ToDto(Movie movie)
        {
            var movieResponse = new MovieResponse
            {
                Id = movie.Id,
                Title = movie.Title,
                ImageUrl = movie.ImageUrl,
                Duration = movie.Duration,
                Description = movie.Description,
                IsPublished = movie.IsPublished ? 
                    "Published" : "Unpublished",
                ReleaseDate = _dateUtil.GetDateToString(movie.ReleaseDate),
                CreatedAt = _dateUtil.GetDateTimeToString(movie?.CreatedAt),
                UpdatedAt = _dateUtil.GetDateTimeToString(movie?.UpdatedAt),

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