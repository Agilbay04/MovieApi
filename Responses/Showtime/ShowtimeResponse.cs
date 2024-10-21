using System.Text.Json.Serialization;

namespace MovieApi.Responses.Showtime
{
    public class ShowtimeResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        
        [JsonPropertyName("movie_id")]
        public string? MovieId { get; set; }
        
        [JsonPropertyName("movie_title")]
        public string? MovieTitle { get; set; }
        
        [JsonPropertyName("studio_id")]
        public string? StudioId { get; set; }
        
        [JsonPropertyName("studio_code")]
        public string? StudioCode { get; set; }
        
        [JsonPropertyName("studio_name")]
        public string? StudioName { get; set; }
        
        [JsonPropertyName("price_code")]
        public string? PriceCode { get; set; }
        
        [JsonPropertyName("price")]
        public string? Price { get; set; }
        
        [JsonPropertyName("start_time")]
        public string? StartTime { get; set; }
        
        [JsonPropertyName("play_date")]
        public string? PlayDate { get; set; }
        
        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }
        
        [JsonPropertyName("updated_at")]
        public string? UpdatedAt { get; set; }
    }
}