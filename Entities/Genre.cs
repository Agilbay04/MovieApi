using System.ComponentModel.DataAnnotations;
using MovieApi.Constants;

namespace MovieApi.Entities
{
    public class Genre
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
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