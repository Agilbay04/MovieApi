using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Entities
{
    public class MovieGenre : BaseEntity
    {
        [ForeignKey("Movie")]
        public string MovieId { get; set; }
        
        [ForeignKey("Genre")]
        public string GenreId { get; set; }

        public virtual Movie? Movie { get; set; }
        
        public virtual Genre? Genre { get; set; }
    }
}