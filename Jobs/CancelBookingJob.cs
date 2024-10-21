using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Utilities;
using Quartz;

namespace MovieApi.Jobs
{
    public class CancelBookingJob : IJob
    {
        private readonly AppDbContext _context;

        public CancelBookingJob(AppDbContext context)
        {
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var bookingToCancel = await _context
                .Bookings
                .Where(b => b.Status == (int)AppConstant.StatusBooking.NEW &&
                    b.PaymentType == AppConstant.PAYMENT_TYPE_TRANSFER &&
                    b.PaymentProof == null &&
                    b.IsPaid == false &&
                    b.BookingDate.AddMinutes(1) > DateTime.Now)
                .ToListAsync();
            
            foreach (var booking in bookingToCancel)
            {
                booking.Status = (int)AppConstant.StatusBooking.CANCEL;
                booking.CancelReason = "Canceled by system because payment expired";   
                _context.Bookings.Update(booking);
            }

            var bookingIds = bookingToCancel.Select(b => b.Id).ToList();
            var bookingSeatsToCancel = await _context
                .BookingSeats
                .Where(bs => bookingIds.Contains(bs.BookingId))
                .ToListAsync();
            foreach (var bookingSeat in bookingSeatsToCancel)
            {
                bookingSeat.Deleted = true;
                _context.BookingSeats.Update(bookingSeat);
            }

            await _context.SaveChangesAsync();
        }
    }
}