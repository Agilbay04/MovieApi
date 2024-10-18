using System.ComponentModel.DataAnnotations;
using MovieApi.Constants;

namespace MovieApi.Entities
{
    public class Movie
    { 
        [Key]
        [MaxLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }
        
        [Required]
        [MaxLength(3)]
        public int Duration { get; set; } = 0;
        
        [Required]
        [MaxLength(1)]
        public int IsPublished { get; set; } = (int)AppConstant.StatusPublished.Published;

        [Required]
        [MaxLength(1)]
        public int Deleted { get; set; } = (int)AppConstant.StatusDelete.NotDeleted;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(100)]
        public string? CreatedBy { get; set; } 

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }
}