using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Requests.Booking;
using MovieApi.Services.UploadService;
using MovieApi.Services.UserService;
using MovieApi.Utilities;

namespace MovieApi.Services.BookingService
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;

        private readonly CodeUtil _codeUtil;

        private readonly IUploadService _uploadService;

        private readonly IUserService _userService;

        public BookingService(
            AppDbContext context, 
            CodeUtil codeUtil, 
            IUploadService uploadService,
            IUserService userService)
        {
            _context = context;
            _codeUtil = codeUtil;
            _uploadService = uploadService;
            _userService = userService;
        }

        public async Task<(Booking, List<string>, User)> BookingFromAdminAsync(CreateBookingRequest req)
        {
            var orderFrom = AppConstant.ORDER_FROM_ADMIN;
            var bookingCode = await _codeUtil.GenerateCode(orderFrom);

            if (req.ShowtimeId == null)
                throw new Exception("Showtime is required");
            
            if (req.Seats == null)
                throw new Exception("Seats is required");

            var showTime = await _context
                .Showtimes
                .FirstOrDefaultAsync(x => x.Id == req.ShowtimeId && x.Deleted == false) ??
                throw new Exception("Showtime not found");

            var booking = new Booking
            {
                ShowtimeId = req.ShowtimeId,
                BookingCode = bookingCode,
                CustomerName = await _codeUtil.GenerateCustomerName(),
                OrderFrom = orderFrom,
                BookingDate = DateTime.Now,
                PaymentType = AppConstant.PAYMENT_TYPE_OTS,
                IsPaid = true,
                Status = (int)AppConstant.StatusBooking.DONE,
                Quantity = req.Seats.Count,
                PriceValue = showTime.Price.PriceValue,
                TotalPrice = showTime.Price.PriceValue * req.Seats.Count,
                CreatedBy = _userService.GetUserId(),
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            foreach (var seat in req.Seats)
            {
                if (!await IsSeatAvailable(seat, showTime.StudioId))
                    throw new Exception("Seat not available");
                
                var setSeatToReserved = _context
                    .Seats
                    .FirstOrDefault(x => x.Id == seat && x.StudioId == showTime.StudioId) ??
                    throw new Exception("Seat not found");
                setSeatToReserved.IsAvailable = false;
                _context.Seats.Update(setSeatToReserved);

                var bookingSeat = new BookingSeat
                {
                    BookingId = booking.Id,
                    SeatId = seat,
                    CreatedBy = _userService.GetUserId()
                };
                await _context.BookingSeats.AddAsync(bookingSeat);
            }

            await _context.SaveChangesAsync();

            var bookingResult = await _context
                .Bookings
                .Where(x => x.Id == booking.Id)
                .FirstOrDefaultAsync() ?? throw new Exception("Booking not found");

            var listSeat = await _context
                .Seats
                .Where(x => req.Seats.Contains(x.Id))
                .Select(x => x.SeatNumber)
                .ToListAsync() ?? throw new Exception("Seats not found");

            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.Id == _userService.GetUserId()) ?? 
                throw new Exception("User not found");

            return (bookingResult, listSeat, user);
        }

        public async Task<(Booking, List<string>, User)> BookingFromCustomerAsync(CreateBookingRequest req)
        {
            var orderFrom = AppConstant.ORDER_FROM_CUSTOMER;
            var bookingCode = await _codeUtil.GenerateCode(orderFrom);
            bool isPaid = false;
            int bookingStatus = (int)AppConstant.StatusBooking.NEW;
            string paymentProof = null;

            if (req.ShowtimeId == null)
                throw new Exception("Showtime is required");
            
            if (req.Seats == null)
                throw new Exception("Seats is required");
            
            if (req.PaymentType != AppConstant.PAYMENT_TYPE_TRANSFER)
                throw new Exception($"Payment type {req.PaymentType} is not available");

            if (req.PaymentProof != null)
            {
                paymentProof = await _uploadService.UploadFileAsync(req.PaymentProof, "Payments");
                bookingStatus = (int)AppConstant.StatusBooking.ON_CONFIRMATION;
            }

            var showTime = await _context
                .Showtimes
                .FirstOrDefaultAsync(x => x.Id == req.ShowtimeId && x.Deleted == false) ??
                throw new Exception("Showtime not found");

            var booking = new Booking
            {
                ShowtimeId = req.ShowtimeId,
                BookingCode = bookingCode,
                CustomerId = _userService.GetUserId(),
                CustomerName = _userService.GetUsername(),
                OrderFrom = orderFrom,
                BookingDate = DateTime.Now,
                PaymentType = req.PaymentType,
                IsPaid = isPaid,
                Status = bookingStatus,
                PaymentProof = paymentProof,
                Quantity = req.Seats.Count,
                PriceValue = showTime.Price.PriceValue,
                TotalPrice = showTime.Price.PriceValue * req.Seats.Count,
                CreatedBy = _userService.GetUserId(),
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            foreach (var seat in req.Seats)
            {
                if (!await IsSeatAvailable(seat, showTime.StudioId))
                    throw new Exception("Seat not available");
                
                var setSeatToReserved = _context
                    .Seats
                    .FirstOrDefault(x => x.Id == seat && x.StudioId == showTime.StudioId) ??
                    throw new Exception("Seat not found");
                setSeatToReserved.IsAvailable = false;
                _context.Seats.Update(setSeatToReserved);

                var bookingSeat = new BookingSeat
                {
                    BookingId = booking.Id,
                    SeatId = seat,
                    CreatedBy = _userService.GetUserId()
                };
                await _context.BookingSeats.AddAsync(bookingSeat);
            }

            await _context.SaveChangesAsync();

            var bookingResult = await _context
                .Bookings
                .Where(x => x.Id == booking.Id)
                .FirstOrDefaultAsync() ?? throw new Exception("Booking not found");

            var listSeat = await _context
                .Seats
                .Where(x => req.Seats.Contains(x.Id))
                .Select(x => x.SeatNumber)
                .ToListAsync() ?? throw new Exception("Seats not found");

            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.Id == _userService.GetUserId()) ??
                throw new Exception("User not found");

            return (bookingResult, listSeat, user);
        }

        private async Task<bool> IsSeatAvailable(string seatId,  string studioId)
        {
            var seat = await _context
                .Seats
                .FirstOrDefaultAsync(x => x.Id == seatId && x.StudioId == studioId) ?? 
                throw new Exception("Seat not found");
            
            return seat.IsAvailable;    
        }
    }
}