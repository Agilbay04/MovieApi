using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Entities
{
    public class Seat : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [ForeignKey("Studio")]
        public string StudioId { get; set; }

        public virtual Studio? Studio { get; set; }

        [MaxLength(4)]
        public string SeatNumber { get; set; }

        [MaxLength(1)]
        public string Row { get; set; }

        [MaxLength(1)]
        public int Column { get; set; }
    }
}