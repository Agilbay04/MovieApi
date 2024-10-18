using System.Text.Json.Serialization;

namespace MovieApi.Requests.Movie
{
    public class UpdateMovieRequest
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public int Duration { get; set; }
        
        public int IsPublished { get; set; }
        
        [JsonIgnore]
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        
        [JsonIgnore]
        public string? UpdatedBy { get; set; } = "admin";
        
        public List<string>? ListOfGenres { get; set; }
    }
}