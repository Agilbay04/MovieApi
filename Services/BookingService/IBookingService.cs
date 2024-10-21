using MovieApi.Entities;
using MovieApi.Requests.Booking;

namespace MovieApi.Services.BookingService
{
    public interface IBookingService
    {
        Task<(Booking, List<string>, User)> FindBookingByIdAsync(string id);

        Task<(Booking, List<string>, User)> FindBookingByCodeAsync(string code);

        Task<List<Booking>> FindAllBookingAsync();

        Task<(Booking, List<string>, User)> BookingFromAdminAsync(CreateBookingRequest request);

        Task<(Booking, List<string>, User)> BookingFromCustomerAsync(CreateBookingRequest request);

        Task<(Booking, List<string>, User)> ConfirmBookingAsync(ConfirmBookingRequest req, string bookingCode);

        Task<(Booking, List<string>, User)> UploadPaymentProofAsync(IFormFile paymentProof, string bookingCode);
    }
}