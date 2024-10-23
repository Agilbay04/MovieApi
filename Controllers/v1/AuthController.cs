using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Auth;
using MovieApi.Responses;
using MovieApi.Responses.Auth;
using MovieApi.Services.AuthService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
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
        [EndpointSummary("Login user")]
        [EndpointDescription("Login user role admin and customer")]
        [Produces("application/json")]
        [ProducesResponseType(type: typeof(BaseResponseApi<LoginResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<LoginResponse>>> Login(UserLoginRequest req)
        {
            var (user, token) = await _authService.Login(req);
            var loginDto = await _authMapper.ToDtoLogin(user, token);
            var res = new BaseResponseApi<LoginResponse>(loginDto, "Login successful");
            return Ok(res);
        }

        [HttpPost("customer/register")]
        [EndpointSummary("Register customer")]
        [EndpointDescription("Register user role customer")]
        [Produces("application/json")]
        [ProducesResponseType(type: typeof(RegisterResponse), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BaseResponseApi<RegisterResponse>>> Register([FromForm] UserRegisterRequest req)
        {
            try
            {
                var user = await _authService.Register(req);
                var registerDto = await _authMapper.ToDtoRegister(user);
                var res = new BaseResponseApi<RegisterResponse>(registerDto, "Login successful");
                return Ok(res);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("admin/register")]
        [EndpointSummary("Register admin")]
        [EndpointDescription("Register user role admin")]
        [Produces("application/json")]
        [ProducesResponseType(type: typeof(BaseResponseApi<RegisterResponse>), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BaseResponseApi<RegisterResponse>>> RegisterAdmin([FromForm] UserRegisterRequest req)
        {
            try
            {
                var user = await _authService.RegisterAdmin(req);
                var registerDto = await _authMapper.ToDtoRegister(user);
                var res = new BaseResponseApi<RegisterResponse>(registerDto, "Register successful");
                return Ok(res);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}