using log4net;
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
        private static readonly ILog _log = LogManager.GetLogger(typeof(BookingService));

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
                    throw new DllNotFoundException("Booking not found");
            }
            else
            {
                booking = await _context
                    .Bookings
                    .FirstOrDefaultAsync(x => x.Id == id) ??
                    throw new DllNotFoundException("Booking not found");
            }

            var listSeatId = await _context
                .BookingSeats
                .Where(x => x.BookingId == booking.Id && x.Deleted == false)
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
                throw new DllNotFoundException("User not found");
            
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
                    throw new DllNotFoundException("Booking not found");
            }
            else
            {
                booking = await _context
                    .Bookings
                    .FirstOrDefaultAsync(x => x.BookingCode == code) ??
                    throw new DllNotFoundException("Booking not found");
            }
            
            var listSeatId = await _context
                .BookingSeats
                .Where(x => x.BookingId == booking.Id && x.Deleted == false)
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
                throw new DllNotFoundException("User not found");
            
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
            var transcation = _context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            _log.Info($"Admin {_userService.GetUsername()} create booking");
            var orderFrom = AppConstant.ORDER_FROM_ADMIN;

            if (req.ShowtimeId == null)
            {
                _log.Error("Showtime is required");
                throw new BadHttpRequestException("Showtime is required");
            }
            
            if (req.Seats == null)
            {
                _log.Error("Seats is required");
                throw new BadHttpRequestException("Seats is required");
            }

            var showTime = await _context
                .Showtimes
                .FirstOrDefaultAsync(x => x.Id == req.ShowtimeId && x.Deleted == false);
            
            if (showTime == null)
            {
                _log.Error("Showtime not found");
                throw new DllNotFoundException("Showtime not found");
            }

            var booking = new Booking
            {
                ShowtimeId = req.ShowtimeId,
                BookingCode = await _codeUtil.GenerateCode(orderFrom),
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
                if (!await IsSeatAvailable(seat, showTime.Id))
                {
                    _log.Error("Seat not available");
                    throw new BadHttpRequestException("Seat not available");
                }

                var bookingSeat = new BookingSeat
                {
                    BookingId = booking.Id,
                    SeatId = seat,
                    CreatedBy = _userService.GetUserId()
                };
                await _context.BookingSeats.AddAsync(bookingSeat);
            }

            await _context.SaveChangesAsync();
            transcation.Commit();

            var bookingResult = await _context
                .Bookings
                .Where(x => x.Id == booking.Id)
                .FirstOrDefaultAsync() ?? throw new DllNotFoundException("Booking not found");

            var listSeat = await _context
                .Seats
                .Where(x => req.Seats.Contains(x.Id))
                .Select(x => x.SeatNumber)
                .ToListAsync() ?? throw new DllNotFoundException("Seats not found");

            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.Id == _userService.GetUserId()) ?? 
                throw new DllNotFoundException("User not found");

            _log.Info($"Admin {_userService.GetUsername()} successfully create booking, booking code: {bookingResult.BookingCode}");
            return (bookingResult, listSeat, user);
        }

        public async Task<(Booking, List<string>, User)> BookingFromCustomerAsync(CreateBookingRequest req)
        {
            var transcation = _context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            _log.Info($"Customer {_userService.GetUsername()} booking tiket movie from customer");
            var orderFrom = AppConstant.ORDER_FROM_CUSTOMER;
            bool isPaid = false;
            int bookingStatus = (int)AppConstant.StatusBooking.NEW;
            string paymentProof = null;

            if (req.ShowtimeId == null)
                throw new BadHttpRequestException("Showtime is required");
            
            if (req.Seats == null)
                throw new BadHttpRequestException("Seats is required");
            
            if (req.PaymentType != AppConstant.PAYMENT_TYPE_TRANSFER)
                throw new BadHttpRequestException($"Payment type {req.PaymentType} is not available");

            if (req.PaymentProof != null)
            {
                paymentProof = await _uploadService.UploadFileAsync(req.PaymentProof, "Payments");
                bookingStatus = (int)AppConstant.StatusBooking.ON_CONFIRMATION;
            }

            var showTime = await _context
                .Showtimes
                .FirstOrDefaultAsync(x => x.Id == req.ShowtimeId && x.Deleted == false) ??
                throw new DllNotFoundException("Showtime not found");

            var booking = new Booking
            {
                ShowtimeId = req.ShowtimeId,
                BookingCode = await _codeUtil.GenerateCode(orderFrom),
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
                if (!await IsSeatAvailable(seat, showTime.Id))
                    throw new DllNotFoundException("Seat not available");

                var bookingSeat = new BookingSeat
                {
                    BookingId = booking.Id,
                    SeatId = seat,
                    CreatedBy = _userService.GetUserId()
                };
                await _context.BookingSeats.AddAsync(bookingSeat);
            }

            await _context.SaveChangesAsync();
            transcation.Commit();

            var bookingResult = await _context
                .Bookings
                .Where(x => x.Id == booking.Id)
                .FirstOrDefaultAsync() ?? throw new DllNotFoundException("Booking not found");

            var listSeat = await _context
                .Seats
                .Where(x => req.Seats.Contains(x.Id))
                .Select(x => x.SeatNumber)
                .ToListAsync() ?? throw new DllNotFoundException("Seats not found");

            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.Id == _userService.GetUserId()) ??
                throw new Exception("User not found");
                
            _log.Info($"Customer {_userService.GetUsername()} success create booking, booking code: {bookingResult.BookingCode}");
            return (bookingResult, listSeat, user);
        }

        public async Task<(Booking, List<string>, User)> ConfirmBookingAsync(ConfirmBookingRequest req, string bookingCode)
        {
            var booking = await _context
                .Bookings
                .FirstOrDefaultAsync(x => x.BookingCode == bookingCode) ??
                throw new DllNotFoundException("Booking not found");
            
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
            
            var bookingSeats = await _context
                .BookingSeats
                .Where(x => x.BookingId == booking.Id && x.Deleted == false)
                .ToListAsync() ?? throw new DllNotFoundException("Booking seats not found");
            
            var listSeat = await _context
                .Seats
                .Where(x => bookingSeats.Select(x => x.SeatId).Contains(x.Id))
                .Select(x => x.SeatNumber)
                .ToListAsync() ?? throw new DllNotFoundException("Seats not found");

            if (!req.IsConfirmed)
            {
                foreach (var seat in bookingSeats)
                {
                    seat.Deleted = true;
                    _context.BookingSeats.Update(seat);
                }
                await _context.SaveChangesAsync();
            }

            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.Id == booking.CreatedBy) ??
                throw new DllNotFoundException("User not found");
            
            return (booking, listSeat, user);
        }

        public async Task<(Booking, List<string>, User)> UploadPaymentProofAsync(IFormFile? paymentProof, string bookingCode)
        {
            if (paymentProof == null)
                throw new BadHttpRequestException("Payment proof is required");
            
            var booking = await _context
                .Bookings
                .FirstOrDefaultAsync(x => x.BookingCode == bookingCode) ??
                throw new BadHttpRequestException("Booking not found");
            
            booking.PaymentProof = await _uploadService.UploadFileAsync(paymentProof, "Payments");
            booking.Status = (int)AppConstant.StatusBooking.ON_CONFIRMATION;
            booking.UpdatedBy = _userService.GetUserId();
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            
            var listSeatId = await _context
                .BookingSeats
                .Where(x => x.BookingId == booking.Id && x.Deleted == false)
                .Select(x => x.SeatId)
                .ToListAsync() ?? throw new DllNotFoundException("Seats not found");

            var listSeat = await _context
                .Seats
                .Where(x => listSeatId.Contains(x.Id))
                .Select(x => x.SeatNumber)
                .ToListAsync() ?? throw new DllNotFoundException("Seats not found");

            var user = await _context
                .Users
                .FirstOrDefaultAsync(x => x.Id == booking.CreatedBy) ??
                throw new DllNotFoundException("User not found");
            
            return (booking, listSeat, user);
        }

        private async Task<bool> IsSeatAvailable(string seatId, string showtimeId)
        {
            var isSeatAvailable = await _context
                .Bookings
                .Join(_context.BookingSeats, b => b.Id, bs => bs.BookingId, (b, bs) => new { b, bs })
                .Where(x => x.bs.SeatId == seatId && x.b.ShowtimeId == showtimeId && x.bs.Deleted == false)
                .FirstOrDefaultAsync();
            
            if (isSeatAvailable != null)
                return false;
            
            return true;    
        }
    }
}