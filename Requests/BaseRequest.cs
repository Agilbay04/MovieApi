using System.Text.Json.Serialization;

namespace MovieApi.Requests
{
    public class BaseRequest
    {
        [JsonIgnore]
        public string? UserId { get; set; }

        [JsonIgnore]
        public string? Username { get; set; }

        [JsonIgnore]
        public string? RoleCode { get; set; }
    }
}