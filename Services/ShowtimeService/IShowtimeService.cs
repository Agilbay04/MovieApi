using MovieApi.Entities;
using MovieApi.Requests.Showtime;

namespace MovieApi.Services.ShowtimeService
{
    public interface IShowtimeService
    {
        Task<Showtime> FindByIdAsync(string id);
        Task<IEnumerable<Showtime>> FindAllAsync();
        Task<Showtime> CreateAsync(CreateShowtimeRequest req);
        Task<Showtime> UpdateAsync(UpdateShowtimeRequest req, string id);
        Task<Showtime> DeleteAsync(string id);
    }
}