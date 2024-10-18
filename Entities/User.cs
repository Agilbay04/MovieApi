using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieApi.Constants;

namespace MovieApi.Entities
{
    public class User
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(128)]
        public string Salt { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [ForeignKey("Role")]
        public string? RoleId { get; set; }

        public virtual Role? Role { get; set; }

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