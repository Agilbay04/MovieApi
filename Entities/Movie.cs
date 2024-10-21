using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace MovieApi.Entities
{
    public class Movie
    { 
        [Key]
        [MaxLength(100)]
        public string Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        
        public string? ImageUrl { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        
        [Required]
        [MaxLength(3)]
        public int Duration { get; set; } = 0;

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public bool IsPublished { get; set; } = false;

        public bool Deleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; } 

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }
}