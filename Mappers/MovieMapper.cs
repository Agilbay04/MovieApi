using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Responses.Movie;
using MovieApi.Utilities;

namespace MovieApi.Mappers
{
    public class MovieMapper
    {
        private readonly DateUtil _dateUtil;

        public MovieMapper(DateUtil dateUtil)
        {
            _dateUtil = dateUtil;
        }

        public async Task<MovieResponse> ToDto(Movie movie, List<Genre>? listGenres)
        {
            return await Task.Run(() => { 
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

                if (listGenres != null)
                {
                    movieResponse.ListOfGenres = listGenres.Select(g => g.Name).ToList();
                }

                return movieResponse;
            });
        }
    }
}