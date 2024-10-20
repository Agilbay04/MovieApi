using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Requests.Price;

namespace MovieApi.Services.PriceService
{
    public class PriceService : IPriceService
    {
        private readonly AppDbContext _context;

        public PriceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Price> FindByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("Id is required");

            var price = await _context.Prices
                .FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false) ?? 
                throw new Exception("Price not found");
            
            return price;
        }

        public async Task<List<Price>> FindAllAsync()
        {
            var prices = await _context
                .Prices
                .Where(p => p.Deleted == false)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return prices;
        }

        public async Task<Price> CreateAsync(CreatePriceRequest req)
        {
            var price = new Price
            {
                Code = req.Code,
                Name = req.Name,
                Description = req.Description,
                PriceValue = req.PriceValue
            };
            await _context.Prices.AddAsync(price);
            await _context.SaveChangesAsync();
            return price;
        }

        public async Task<Price> UpdateAsync(UpdatePriceRequest req, string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("Id is required");

            var price = await FindByIdAsync(id);
            price.Code = req.Code;
            price.Name = req.Name;
            price.Description = req.Description;
            price.PriceValue = req.PriceValue;
            _context.Prices.Update(price);
            await _context.SaveChangesAsync();
            return price;
        }

        public async Task<Price> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new Exception("Id is required");

            var price = await FindByIdAsync(id);
            price.Deleted = true;
            _context.Prices.Update(price);
            await _context.SaveChangesAsync();
            return price;
        }
    }
}