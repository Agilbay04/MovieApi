using MovieApi.Entities;
using MovieApi.Requests.Studio;

namespace MovieApi.Services.StudioService
{
    public interface IStudioService
    {
        Task<(Studio, IEnumerable<Seat>)> FindByIdAsync(string id);
        
        Task<IEnumerable<Studio>> FindAllAsync();

        Task<(Studio, IEnumerable<Seat>)> CreateAsync(CreateStudioRequest req);

        Task<(Studio, IEnumerable<Seat>)> UpdateAsync(UpdateStudioRequest req, string id);

        Task<Studio> DeleteAsync(string id);
    }
}