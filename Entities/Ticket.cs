using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Entities
{
    public class Ticket
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        [ForeignKey("Showtime")]
        public string ShowtimeId { get; set; }

        public virtual Showtime? Showtime { get; set; }

        [Required]
        [MaxLength(100)]
        [ForeignKey("Price")]
        public string PriceId { get; set; }

        public virtual Price? Price { get; set; }

        public bool Deleted { get; set; }

        public DateTime CreatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }
}