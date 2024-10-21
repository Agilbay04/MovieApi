using MovieApi.Entities;
using MovieApi.Requests.Showtime;
using MovieApi.Responses.Seat;

namespace MovieApi.Services.ShowtimeService
{
    public interface IShowtimeService
    {
        Task<(Showtime, List<SeatResponse>)> FindByIdAsync(string id);
        Task<IEnumerable<Showtime>> FindAllAsync();
        Task<Showtime> CreateAsync(CreateShowtimeRequest req);
        Task<Showtime> UpdateAsync(UpdateShowtimeRequest req, string id);
        Task<Showtime> DeleteAsync(string id);
    }
}