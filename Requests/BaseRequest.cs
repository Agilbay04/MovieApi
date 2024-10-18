using System.Security.Claims;
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

    public static class BaseRequestExtensions
    {
        public static void SetUser(this BaseRequest request, ClaimsPrincipal user)
        {
            request.UserId = user.FindFirst("id")?.Value;
            request.Username = user.FindFirst("username")?.Value;
            request.RoleCode = user.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}