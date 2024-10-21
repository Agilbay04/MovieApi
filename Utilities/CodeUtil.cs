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
                .Select(b => b.BookingCode)
                .ToListAsync();
            string bookingCode = "";
            
            if (orderFrom == AppConstant.ORDER_FROM_ADMIN)
            {
                bookings = bookings.Where(b => b.StartsWith("TX1A")).ToList();
                bookingCode = $"TX1A{bookings.Count + 1}";
            }
            else if (orderFrom == AppConstant.ORDER_FROM_CUSTOMER)
            {
                bookings = bookings.Where(b => b.StartsWith("TX2C")).ToList();
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