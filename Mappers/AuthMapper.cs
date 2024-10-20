using MovieApi.Entities;
using MovieApi.Responses.Auth;

namespace MovieApi.Mappers
{
    public class AuthMapper
    {
        public async Task<LoginResponse> ToDtoLogin(User user, string token)
        {
            return await Task.Run(() =>
            {
                return new LoginResponse
                {
                    Username = user.Username,
                    Role = user.Role?.Code,
                    Token = token
                };
            });
        }

        public async Task<RegisterResponse> ToDtoRegister(User user)
        {
            return await Task.Run(() =>
            {
                return new RegisterResponse
                {
                    Username = user.Username,
                    Name = user.Name
                };
            });
        }
    }
}