using System.ComponentModel.DataAnnotations;

namespace MovieApi.Requests.Role
{
    public class CreateRoleRequest
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }
    }
}