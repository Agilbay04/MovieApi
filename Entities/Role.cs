using System.ComponentModel.DataAnnotations;

namespace MovieApi.Entities
{
    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(30)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}