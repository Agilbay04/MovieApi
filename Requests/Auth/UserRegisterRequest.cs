using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieApi.Requests.Auth
{
    public class UserRegisterRequest
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("profile_picture")]
        public IFormFile? ProfilePicture { get; set; }
        
        [Required]
        [MinLength(5)]
        [JsonPropertyName("username")]
        public string Username { get; set; }
        
        [Required]
        [MinLength(8)]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}