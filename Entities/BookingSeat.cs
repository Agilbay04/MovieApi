using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Entities
{
    public class BookingSeat
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string BookingId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SeatId { get; set; }

        public virtual Booking? Booking { get; set; }

        public virtual Seat? Seat { get; set; }

        public bool Deleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }
}