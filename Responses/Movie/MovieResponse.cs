using System.Text.Json.Serialization;

namespace MovieApi.Responses.Movie
{
    public class MovieResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("duration")]
        public int? Duration { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("is_published")]
        public string? IsPublished { get; set; }

        [JsonPropertyName("list_of_genres")]
        public List<string> ListOfGenres { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public string? UpdatedAt { get; set; }
    }
}