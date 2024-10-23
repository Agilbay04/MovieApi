using System.ComponentModel.DataAnnotations;

namespace MovieApi.Entities
{
    public class Price : BaseEntity
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(8)]
        public int PriceValue { get; set; }
    }
}