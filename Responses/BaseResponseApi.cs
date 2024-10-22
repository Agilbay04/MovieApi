using System.Text.Json.Serialization;

namespace MovieApi.Responses
{
    public class BaseResponseApi<T>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        public BaseResponseApi(T data, string? message = null)
        {
            Success = true;
            Message = message ?? "Request was successful";
            Data = data;
        }
    }
}