using log4net;
using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Exceptions;
using MovieApi.Requests.Auth;
using MovieApi.Services.UploadService;
using MovieApi.Utilities;

namespace MovieApi.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(AuthService));
        
        private readonly AppDbContext _context;

        private readonly JwtUtil _jwtUtil;

        private readonly IUploadService _uploadService;


        public AuthService(AppDbContext context, JwtUtil jwtUtil, IUploadService uploadService)
        {
            _context = context;
            _jwtUtil = jwtUtil;
            _uploadService = uploadService;
        }

        public async Task<(User, string)> Login(UserLoginRequest req)
        {
            string token = "";
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == req.Username && 
                    x.Deleted == false);

            if (user == null)
            {
                _log.Error($"User not found: {req.Username}");
                throw new NotFoundException("User not found");
            } 

            if (PasswordUtil.VerifyPassword(req.Password, user.Password, user.Salt))
            {
                token = _jwtUtil.GenerateJwtToken(user);
            }
            else
            {
                _log.Error($"Invalid username or password: {req.Username}");
                throw new BadRequestException("Invalid username or password");
            }

            _log.Info($"User role {user.Role.Code} login to the system: {user.Username}");
            return (user, token);
        }

        public async Task<User> Register(UserRegisterRequest req)
        {
            var roleId = "";
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == req.Username && 
                    x.Deleted == false);
            string imageUrl = "";

            if (req.ProfilePicture != null)
            {
                imageUrl = await _uploadService.UploadFileAsync(req.ProfilePicture, "Customers");
            }

            if (user != null)
            {
                _log.Error($"Username already taken by another user: {req.Username}");
                throw new BadRequestException("Username already taken by another user");
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
                ImageUrl = imageUrl
            };
            
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            _log.Info($"Register new user: {newUser.Username}");
            return newUser;
        }

        public async Task<User> RegisterAdmin(UserRegisterRequest req)
        {
            var roleId = "";
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == req.Username && 
                    x.Deleted == false);
            var imageUrl = "";

            if (req.ProfilePicture != null)
            {
                imageUrl = await _uploadService.UploadFileAsync(req.ProfilePicture, "Admins");
            }

            if (user != null)
            {
                _log.Error($"Username already taken by another user: {req.Username}");
                throw new BadRequestException("Username already taken by another user");
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
                ImageUrl = imageUrl
            };
            
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            _log.Info($"Register new admin: {newUser.Username}");
            return newUser;
        }

    }
}