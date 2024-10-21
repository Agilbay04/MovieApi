using System.Security.Claims;

namespace MovieApi.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId() => _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value ?? "";

        public string GetUsername() => _httpContextAccessor.HttpContext?.User?.FindFirst("username")?.Value ?? "";

        public string GetRoleCode() => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? "";
    }
}