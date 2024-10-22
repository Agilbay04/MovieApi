using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieApi.Requests.Auth
{
    public class UserLoginRequest
    {
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