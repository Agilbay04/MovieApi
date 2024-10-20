using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Studio;
using MovieApi.Responses.Studio;
using MovieApi.Services.StudioService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudioController : ControllerBase
    {
        private readonly IStudioService _studioService;
        private readonly StudioMapper _studioMapper;

        public StudioController(IStudioService studioService, StudioMapper studioMapper)
        {
            _studioService = studioService;
            _studioMapper = studioMapper;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(StudioResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<StudioResponse>> FindStudioByIdAsync(string id)
        {
            try
            {
                var (studio, seats) = await _studioService.FindByIdAsync(id);
                var studioDto = await _studioMapper.ToDtoWithSeats(studio, seats.ToList());
                return Ok(studioDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(List<StudioResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<List<StudioResponse>>> FindAllStudiosAsync()
        {
            try
            {
                var studios = await _studioService.FindAllAsync();
                var studioDtos = await _studioMapper.ToDtos(studios.ToList());
                return Ok(studioDtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(StudioResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<StudioResponse>> CreateStudioAsync(CreateStudioRequest req)
        {
            try
            {
                var (studio, seats) = await _studioService.CreateAsync(req);
                var studioDto = await _studioMapper.ToDtoWithSeats(studio, seats.ToList());
                return Ok(studioDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(StudioResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<StudioResponse>> UpdateStudioAsync(UpdateStudioRequest req, string id)
        {
            try
            {
                var (studio, seats) = await _studioService.UpdateAsync(req, id);
                var studioDto = await _studioMapper.ToDtoWithSeats(studio, seats.ToList());
                return Ok(studioDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(StudioResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<StudioResponse>> DeleteStudioAsync(string id)
        {
            try
            {
                var studio = await _studioService.DeleteAsync(id);
                var studioDto = await _studioMapper.ToDto(studio);
                return Ok(studioDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}