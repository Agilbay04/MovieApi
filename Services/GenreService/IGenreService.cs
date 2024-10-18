using MovieApi.Entities;
using MovieApi.Requests.Genre;

namespace MovieApi.Services.GenreService
{
    public interface IGenreService
    {
        Task<Genre> FindByIdAsync(string id);
        Task<IEnumerable<Genre>> FindAllAsync();
        Task<Genre> CreateAsync(CreateGenreRequest req);
        Task<Genre> UpdateAsync(UpdateGenreRequest req, string id);
        Task<Genre> DeleteAsync(string id);
    }
}