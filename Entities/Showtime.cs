using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Entities
{
    public class Showtime
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        [ForeignKey("Movie")]
        public string MovieId { get; set; }

        [Required]
        [MaxLength(100)]
        [ForeignKey("Studio")]
        public string StudioId { get; set; }

        [Required]
        [MaxLength(100)]
        [ForeignKey("Price")]
        public string PriceId { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public DateTime PlayDate { get; set; }

        public virtual Movie? Movie { get; set; }

        public virtual Studio? Studio { get; set; }

        public virtual Price? Price { get; set; }

        public bool Deleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }
}