using System.ComponentModel.DataAnnotations;

namespace MovieApi.Entities
{
    public class Genre
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        public bool Deleted { get; set; }
        
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        
        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }
}