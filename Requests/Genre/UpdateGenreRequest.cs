using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieApi.Requests.Genre
{
    public class UpdateGenreRequest
    {
        [Required]
        public string Name { get; set; }
        
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}