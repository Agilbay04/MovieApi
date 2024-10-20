using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieApi.Constants;

namespace MovieApi.Entities
{
    public class MovieGenre
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [ForeignKey("Movie")]
        public string MovieId { get; set; }
        
        [ForeignKey("Genre")]
        public string GenreId { get; set; }

        public virtual Movie? Movie { get; set; }
        
        public virtual Genre? Genre { get; set; }

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