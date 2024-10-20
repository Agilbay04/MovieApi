using System.Text.Json.Serialization;

namespace MovieApi.Requests
{
    public class CreateMovieRequest
    {
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public int Duration { get; set; } = 0;

        public string ReleaseDate { get; set; }
        
        public bool IsPublished { get; set; }
        
        public List<string>? ListOfGenres { get; set; }
    }
}