using System.Text.Json.Serialization;

namespace MovieApi.Responses.Role
{
    public class RoleResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        
        [JsonPropertyName("code")]
        public string? Code { get; set; }
        
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        
        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }
        
        [JsonPropertyName("updated_at")]
        public string? UpdatedAt { get; set; }
    }
}