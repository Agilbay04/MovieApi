using System.ComponentModel.DataAnnotations;

namespace MovieApi.Requests.Booking
{
    public class EditBookingRequest
    {
        public string? OrderFrom { get; set; }

        public string? PaymentType { get; set; }

        public IFormFile? PaymentProof { get; set; }
    }
}