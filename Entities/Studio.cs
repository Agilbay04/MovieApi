using System.ComponentModel.DataAnnotations;

namespace MovieApi.Entities
{
    public class Studio : BaseEntity
    {
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
    }
}