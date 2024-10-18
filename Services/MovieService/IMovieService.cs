using MovieApi.Entities;
using MovieApi.Requests;
using MovieApi.Requests.Movie;

namespace MovieApi.Services.MovieService
{
    public interface IMovieService
    {
        Task<Movie> FindByIdAsync(string id);
        Task<IEnumerable<Movie>> FindAllAsync();
        Task<Movie> CreateAsync(CreateMovieRequest req);
        Task<Movie> UpdateAsync(UpdateMovieRequest req, string id);
        Task<Movie> DeleteAsync(string id);
    }
}