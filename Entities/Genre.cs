using System.ComponentModel.DataAnnotations;

namespace MovieApi.Entities
{
    public class Genre : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}