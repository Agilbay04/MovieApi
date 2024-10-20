using System.ComponentModel.DataAnnotations;

namespace MovieApi.Entities
{
    public class Studio
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Facility { get; set; }
        
        [Required]
        [MaxLength(3)]
        public int TotalSeats { get; set; } = 0;

        public bool Deleted { get; set; } = false;

        public DateTime? CreatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }
}