using Microsoft.EntityFrameworkCore;
using MovieApi.Database;

namespace MovieApi.Utilities
{
    public class CodeUtil
    {
        private readonly AppDbContext _context;

        public CodeUtil(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateCode(string orderFrom)
        {
            var bookings = await _context.Bookings
                .Select(b => b.Id)
                .ToListAsync();
            string bookingCode = "";
            
            if (orderFrom == AppConstant.ORDER_FROM_ADMIN)
            {
                bookingCode = $"TX1A{bookings.Count + 1}";
            }
            else if (orderFrom == AppConstant.ORDER_FROM_CUSTOMER)
            {
                bookingCode = $"TX2C{bookings.Count + 1}";
            }

            return bookingCode;
        }

        public async Task<string> GenerateCustomerName()
        {
            var bookings = await _context.Bookings
                .Select(b => b.Id)
                .ToListAsync();

            if (bookings.Count == 0)
                return "Customer-1";

            return $"Customer-{bookings.Count + 1}";
        }
    }
}