using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Constants;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Requests.Auth;
using MovieApi.Utilities;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtUtil _jwtUtil;

        private readonly AppDbContext _context;

        public AuthController(JwtUtil jwtUtil, AppDbContext context)
        {
            _jwtUtil = jwtUtil;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest req)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == req.Username && 
                    x.Deleted == (int)AppConstant.StatusDelete.NotDeleted);
                    

            if (user == null)
            {
                return NotFound("User not registered");
            }

            if (PasswordUtil.VerifyPassword(req.Password, user.Password, user.Salt))
            {
                var token = _jwtUtil.GenerateJwtToken(user);
                return Ok(new { token });
            }
            else
            {
                return BadRequest("Invalid username or password");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest req)
        {
            var roleId = "";
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == req.Username && 
                    x.Deleted == (int)AppConstant.StatusDelete.NotDeleted);

            if (user != null)
            {
                return BadRequest("Username already exists");
            }

            var hashedPassword = PasswordUtil.HashPassword(req.Password);

            var roleCustomer = await _context.Roles
                .FirstOrDefaultAsync(x => x.Code == AppConstant.ROLE_CUSTOMER && 
                    x.Deleted == (int)AppConstant.StatusDelete.NotDeleted);

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
            return Ok();
        }
    }
}