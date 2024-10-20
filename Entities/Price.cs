using System.ComponentModel.DataAnnotations;

namespace MovieApi.Entities
{
    public class Price
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(8)]
        public int PriceValue { get; set; }

        public bool Deleted { get; set; }

        public DateTime? CreatedAt { get; set; }

        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        
        public DateTime? UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }
}