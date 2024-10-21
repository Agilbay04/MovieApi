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

        public async Task<(Booking, List<string>, User)> FindBookingByIdAsync(string id)
        {
            var booking = new Booking();

            if (_userService.GetRoleCode() == AppConstant.ROLE_CUSTOMER)
            {
                booking = await _context
                    .Bookings
                    .Where(x => x.CustomerId == _userService.GetUserId())
                    .FirstOrDefaultAsync(x => x.Id == id) ??
                    throw new Exception("Booking not found");
            }
            else
            {
                booking = await _context
                    .Bookings
                    .FirstOrDefaultAsync(x => x.Id == id) ??
                    throw new Exception("Booking not found");
            }

            var listSeatId = await _context
                .BookingSeats
                .Where(x => x.BookingId == booking.Id)
                .Select(x => x.SeatId)
                .ToListAsync();

            var seats = await _context
                .Seats
                .Where(x => listSeatId.Contains(x.Id))
                .Select(x => x.SeatNumber)
                .ToListAsync();

            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.Id == booking.CreatedBy) ?? 
                throw new Exception("User not found");
            
            return (booking, seats, user);
        }

        public async Task<(Booking, List<string>, User)> FindBookingByCodeAsync(string code)
        {
            var booking = new Booking();

            if (_userService.GetRoleCode() == AppConstant.ROLE_CUSTOMER)
            {
                booking = await _context
                    .Bookings
                    .Where(x => x.CustomerId == _userService.GetUserId())
                    .FirstOrDefaultAsync(x => x.BookingCode == code) ??
                    throw new Exception("Booking not found");
            }
            else
            {
                booking = await _context
                    .Bookings
                    .FirstOrDefaultAsync(x => x.BookingCode == code) ??
                    throw new Exception("Booking not found");
            }
            
            var listSeatId = await _context
                .BookingSeats
                .Where(x => x.BookingId == booking.Id)
                .Select(x => x.SeatId)
                .ToListAsync();

            var seats = await _context
                .Seats
                .Where(x => listSeatId.Contains(x.Id))
                .Select(x => x.SeatNumber)
                .ToListAsync();

            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.Id == booking.CreatedBy) ?? 
                throw new Exception("User not found");
            
            return (booking, seats, user);
        }

        public async Task<List<Booking>> FindAllBookingAsync()
        {
            var bookings = await _context
                .Bookings
                .ToListAsync();

            if (_userService.GetRoleCode() == AppConstant.ROLE_CUSTOMER)
            {
                bookings = await _context
                    .Bookings
                    .Where(x => x.CustomerId == _userService.GetUserId())
                    .ToListAsync();
            }

            return bookings;
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

        public async Task<(Booking, List<string>, User)> ConfirmBookingAsync(ConfirmBookingRequest req, string bookingCode)
        {
            var booking = await _context
                .Bookings
                .FirstOrDefaultAsync(x => x.BookingCode == bookingCode) ??
                throw new Exception("Booking not found");
            
            if (!req.IsConfirmed)
            {
                booking.IsPaid = false;
                booking.Status = (int)AppConstant.StatusBooking.CANCEL;
                booking.CancelReason = req.CancelReason;
            }
            else
            {
                booking.IsPaid = true;
                booking.Status = (int)AppConstant.StatusBooking.DONE;
            }

            booking.UpdatedBy = _userService.GetUserId();
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            
            var listSeatId = await _context
                .BookingSeats
                .Where(x => x.BookingId == booking.Id)
                .Select(x => x.SeatId)
                .ToListAsync() ?? throw new Exception("Booking seats not found");

            var listSeat = await _context
                .Seats
                .Where(x => listSeatId.Contains(x.Id))
                .ToListAsync() ?? throw new Exception("Seats not found");
            
            if (!req.IsConfirmed)
            {
                foreach (var seat in listSeat)
                {
                    seat.IsAvailable = true;
                    _context.Seats.Update(seat);
                    await _context.SaveChangesAsync();
                }
            }

            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.Id == booking.CreatedBy) ??
                throw new Exception("User not found");
            
            return (booking, listSeat.Select(x => x.SeatNumber).ToList(), user);
        }

        public async Task<(Booking, List<string>, User)> UploadPaymentProofAsync(IFormFile? paymentProof, string bookingCode)
        {
            if (paymentProof == null)
                throw new Exception("Payment proof is required");
            
            var booking = await _context
                .Bookings
                .FirstOrDefaultAsync(x => x.BookingCode == bookingCode) ??
                throw new Exception("Booking not found");
            
            booking.PaymentProof = await _uploadService.UploadFileAsync(paymentProof, "Payments");
            booking.Status = (int)AppConstant.StatusBooking.ON_CONFIRMATION;
            booking.UpdatedBy = _userService.GetUserId();
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            
            var listSeatId = await _context
                .BookingSeats
                .Where(x => x.BookingId == booking.Id)
                .Select(x => x.SeatId)
                .ToListAsync() ?? throw new Exception("Seats not found");

            var listSeat = await _context
                .Seats
                .Where(x => listSeatId.Contains(x.Id))
                .Select(x => x.SeatNumber)
                .ToListAsync() ?? throw new Exception("Seats not found");

            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.Id == booking.CreatedBy) ??
                throw new Exception("User not found");
            
            return (booking, listSeat, user);
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