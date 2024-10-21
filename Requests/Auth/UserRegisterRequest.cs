using System.ComponentModel.DataAnnotations;

namespace MovieApi.Requests.Auth
{
    public class UserRegisterRequest
    {
        [Required]
        public string Name { get; set; }

        public IFormFile? ProfilePicture { get; set; }
        
        [Required]
        [MinLength(5)]
        public string Username { get; set; }
        
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}