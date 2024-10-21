using MovieApi.Entities;
using MovieApi.Requests.Booking;

namespace MovieApi.Services.BookingService
{
    public interface IBookingService
    {
        Task<(Booking, List<string>)> BookingFromAdminAsync(CreateBookingRequest request);

        Task<(Booking, List<string>)> BookingFromCustomerAsync(CreateBookingRequest request);
    }
}