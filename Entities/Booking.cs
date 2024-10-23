using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Entities
{
    public class Booking : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string BookingCode { get; set; }

        [Required]
        [MaxLength(100)]
        [ForeignKey("Showtime")]
        public string ShowtimeId { get; set; }

        public virtual Showtime? Showtime { get; set; }

        [MaxLength(100)]
        [ForeignKey("User")]
        public string? CustomerId { get; set; }

        [MaxLength(100)]
        public string? CustomerName { get; set; }

        public virtual User? User { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        [MaxLength(10)]
        public string PaymentType { get; set; }
        
        [Required]
        public bool IsPaid { get; set; }

        [Required]
        [MaxLength(1)]
        public int? Status { get; set; }

        public string? PaymentProof { get; set; }

        [Required]
        [MaxLength(4)]
        public int Quantity { get; set; }

        [Required]
        public int PriceValue { get; set; }

        [Required]
        public int TotalPrice { get; set; }

        public string? CancelReason { get; set; }

        [Required]
        [MaxLength(20)]
        public string OrderFrom { get; set; }
    }
}