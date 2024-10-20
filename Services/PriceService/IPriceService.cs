using MovieApi.Entities;
using MovieApi.Requests.Price;

namespace MovieApi.Services.PriceService
{
    public interface IPriceService
    {
        Task<Price> FindByIdAsync(string id);
        Task<List<Price>> FindAllAsync();
        Task<Price> CreateAsync(CreatePriceRequest req);
        Task<Price> UpdateAsync(UpdatePriceRequest req, string id);
        Task<Price> DeleteAsync(string id);
    }
}