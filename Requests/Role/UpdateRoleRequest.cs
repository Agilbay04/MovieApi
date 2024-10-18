using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieApi.Requests.Role
{
    public class UpdateRoleRequest
    {
        [Required]
        public string Code { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}