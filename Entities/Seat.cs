using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Entities
{
    public class Seat
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        [ForeignKey("Studio")]
        public string StudioId { get; set; }

        public virtual Studio? Studio { get; set; }

        [MaxLength(4)]
        public string SeatNumber { get; set; }

        [MaxLength(1)]
        public string Row { get; set; }

        [MaxLength(1)]
        public int Column { get; set; }

        [Required]
        public bool IsAvailable { get; set; } = true;

        public bool Deleted { get; set; } = false;

        public DateTime? CreatedAt { get; set; }
        
        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }
}