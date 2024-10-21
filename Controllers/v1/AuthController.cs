using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Auth;
using MovieApi.Responses.Auth;
using MovieApi.Services.AuthService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        private readonly AuthMapper _authMapper;

        public AuthController(IAuthService authService, AuthMapper authMapper)
        {
            _authService = authService;
            _authMapper = authMapper;
        }

        [HttpPost("login")]
        [ProducesResponseType(type: typeof(LoginResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<LoginResponse>> Login(UserLoginRequest req)
        {
            try
            {
                var (user, token) = await _authService.Login(req);
                var loginDto = await _authMapper.ToDtoLogin(user, token);
                return Ok(loginDto);

            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost("customer/register")]
        [ProducesResponseType(type: typeof(RegisterResponse), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<RegisterResponse>> Register([FromForm] UserRegisterRequest req)
        {
            try
            {
                var user = await _authService.Register(req);
                var registerDto = await _authMapper.ToDtoRegister(user);
                return Ok(registerDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("admin/register")]
        [ProducesResponseType(type: typeof(RegisterResponse), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<RegisterResponse>> RegisterAdmin([FromForm] UserRegisterRequest req)
        {
            try
            {
                var user = await _authService.RegisterAdmin(req);
                var registerDto = await _authMapper.ToDtoRegister(user);
                return Ok(registerDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}