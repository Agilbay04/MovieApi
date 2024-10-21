using MovieApi.Entities;
using MovieApi.Responses.Booking;
using MovieApi.Utilities;

namespace MovieApi.Mappers
{
    public class BookingMapper
    {
        private readonly DateUtil _dateUtil;

        private readonly PriceUtil _priceUtil;

        public BookingMapper(DateUtil dateUtil, PriceUtil priceUtil)
        {
            _dateUtil = dateUtil;
            _priceUtil = priceUtil;
        }

        public async Task<BookingResponse> ToDto(Booking booking, List<string>? seats, User? user)
        {
            return await Task.Run(() =>
            {
                return new BookingResponse
                {
                    BookingCode = booking.BookingCode,
                    CustomerName = booking.CustomerName,
                    MovieTitle = booking.Showtime?.Movie?.Title,
                    StartTime = _dateUtil.GetTimeToString(booking.Showtime?.StartTime),
                    PlayDate = _dateUtil.GetDateToString(booking.Showtime?.PlayDate),
                    PaymentStatus = booking.IsPaid ? "Paid" : "Unpaid",
                    Status = GetBookingStatus(booking.Status),
                    PaymentType = booking.PaymentType,
                    Quantity = booking.Quantity,
                    PriceValue = _priceUtil.GetIDRCurrency(booking.Showtime?.Price?.PriceValue ?? 0),
                    TotalPrice = _priceUtil.GetIDRCurrency(booking.TotalPrice),
                    BookingDate = _dateUtil.GetDateToString(booking.BookingDate),
                    Seats = seats,
                    CreatedAt = _dateUtil.GetDateToString(booking.CreatedAt),
                    CreatedBy = user?.Username,
                    UpdatedAt = _dateUtil.GetDateToString(booking.UpdatedAt),
                    UpdatedBy = user?.Username
                };
            });
        }

        public async Task<List<BookingResponse>> ToDtos(List<Booking> bookings)
        {
            return await Task.Run(() =>
            {
                return bookings.Select(booking => ToDto(booking, null, null).Result).ToList();
            });
        }

        private static string GetBookingStatus(int? paymentStatus)
        {
            return paymentStatus switch
            {
                -1 => "CANCEL",
                0 => "NEW",
                1 => "ON CONFIRMATION",
                2 => "DONE",
                _ => "Unknown"
            };
        }
    }
}