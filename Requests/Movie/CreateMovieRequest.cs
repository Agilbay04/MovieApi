using System.Text.Json.Serialization;

namespace MovieApi.Requests
{
    public class CreateMovieRequest
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public int Duration { get; set; } = 0;
        
        public int IsPublished { get; set; } = 1;
        
        [JsonIgnore]
        public string? CreatedBy { get; set; } = "admin";
        
        public List<string>? ListOfGenres { get; set; }
    }
}