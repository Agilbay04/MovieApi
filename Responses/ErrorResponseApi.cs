using System.Text.Json.Serialization;

namespace MovieApi.Responses
{
    public class ErrorResponseApi
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }    
        
        public ErrorResponseApi(string? message, List<string>? error = null)
        {
            Success = false;
            Errors = error ?? new List<string>();
            Message = message ?? "Request was not successful";
        }
    }
}