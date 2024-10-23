using MovieApi.Entities;
using MovieApi.Requests;
using MovieApi.Requests.Movie;
using MovieApi.Responses.Movie;

namespace MovieApi.Services.MovieService
{
    public interface IMovieService
    {
        Task<(Movie, List<Genre>)> FindByIdAsync(string id);
        Task<List<MovieResponse>> FindAllAsync();
        Task<(Movie, List<Genre>)> CreateAsync(CreateMovieRequest req);
        Task<(Movie, List<Genre>)> UpdateAsync(UpdateMovieRequest req, string id);
        Task<(Movie, List<Genre>)> DeleteAsync(string id);
    }
}