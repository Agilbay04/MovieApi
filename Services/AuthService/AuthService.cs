using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Requests.Auth;
using MovieApi.Utilities;

namespace MovieApi.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        private readonly JwtUtil _jwtUtil;

        public AuthService(AppDbContext context, JwtUtil jwtUtil)
        {
            _context = context;
            _jwtUtil = jwtUtil;
        }

        public async Task<(User, string)> Login(UserLoginRequest req)
        {
            string token = "";
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == req.Username && 
                    x.Deleted == false) ?? throw new UnauthorizedAccessException("User not registered");

            if (PasswordUtil.VerifyPassword(req.Password, user.Password, user.Salt))
            {
                token = _jwtUtil.GenerateJwtToken(user);
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            return (user, token);
        }

        public async Task<User> Register(UserRegisterRequest req)
        {
            var roleId = "";
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == req.Username && 
                    x.Deleted == false);

            if (user != null)
            {
                throw new Exception("Username already taken by another user");
            }

            var hashedPassword = PasswordUtil.HashPassword(req.Password);

            var roleCustomer = await _context.Roles
                .FirstOrDefaultAsync(x => x.Code == AppConstant.ROLE_CUSTOMER && 
                    x.Deleted == false);

            if (roleCustomer != null)
            {
                roleId = roleCustomer.Id;
            }

            var newUser = new User
            {
                Name = req.Name,
                Username = req.Username,
                Salt = hashedPassword.salt,
                Password = hashedPassword.hashedPassword,
                RoleId = roleId,
            };
            
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<User> RegisterAdmin(UserRegisterRequest req)
        {
            var roleId = "";
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == req.Username && 
                    x.Deleted == false);

            if (user != null)
            {
                throw new Exception("Username already taken by another user");
            }

            var hashedPassword = PasswordUtil.HashPassword(req.Password);

            var roleAdmin = await _context.Roles
                .FirstOrDefaultAsync(x => x.Code == AppConstant.ROLE_ADMIN && 
                    x.Deleted == false);

            if (roleAdmin != null)
            {
                roleId = roleAdmin.Id;
            }

            var newUser = new User
            {
                Name = req.Name,
                Username = req.Username,
                Salt = hashedPassword.salt,
                Password = hashedPassword.hashedPassword,
                RoleId = roleId,
            };
            
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

    }
}