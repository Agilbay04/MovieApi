namespace MovieApi.Responses.Auth
{
    public class LoginResponse
    {
        public string? Username { get; set; }

        public string? Role { get; set; }

        public string? Token { get; set; }
    }

    public class RegisterResponse
    {
        public string? Username { get; set; }

        public string? Name { get; set; }
    }
}