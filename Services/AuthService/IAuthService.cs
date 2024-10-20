using MovieApi.Entities;
using MovieApi.Requests.Auth;

namespace MovieApi.Services.AuthService
{
    public interface IAuthService
    {
        Task<(User, string)> Login(UserLoginRequest req);

        Task<User> Register(UserRegisterRequest req);

        Task<User> RegisterAdmin(UserRegisterRequest req);
    }
}