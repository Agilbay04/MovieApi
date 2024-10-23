using System.ComponentModel.DataAnnotations;

namespace MovieApi.Entities
{
    public class BookingSeat : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string BookingId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SeatId { get; set; }

        public virtual Booking? Booking { get; set; }

        public virtual Seat? Seat { get; set; }
    }
}