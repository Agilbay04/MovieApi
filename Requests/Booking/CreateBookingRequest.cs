using System.ComponentModel.DataAnnotations;

namespace MovieApi.Requests.Booking
{
    public class CreateBookingRequest
    {
        [Required]
        [MaxLength(100)]
        public string ShowtimeId { get; set; }

        public List<string>? Seats { get; set; }

        public string? PaymentType { get; set; }

        public IFormFile? PaymentProof { get; set; }
    }
}