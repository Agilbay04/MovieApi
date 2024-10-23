using System.ComponentModel.DataAnnotations;

namespace MovieApi.Entities
{
    public class Movie : BaseEntity
    { 
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
    }
}