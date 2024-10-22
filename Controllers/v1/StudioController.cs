using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Studio;
using MovieApi.Responses;
using MovieApi.Responses.Studio;
using MovieApi.Services.StudioService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
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
        [EndpointSummary("Find studio by id")]
        [EndpointDescription("Find studio by id from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<StudioResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<StudioResponse>>> FindStudioByIdAsync(string id)
        {
            try
            {
                var (studio, seats) = await _studioService.FindByIdAsync(id);
                var studioDto = await _studioMapper.ToDtoWithSeats(studio, seats.ToList());
                var res = new BaseResponseApi<StudioResponse>(studioDto, "Get studio success");
                return Ok(studioDto);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpGet]
        [EndpointSummary("Find all studios")]
        [EndpointDescription("Find all studios from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<List<StudioResponse>>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<List<StudioResponse>>>> FindAllStudiosAsync()
        {
            try
            {
                var studios = await _studioService.FindAllAsync();
                var studioDtos = await _studioMapper.ToDtos(studios.ToList());
                var res = new BaseResponseApi<List<StudioResponse>>(studioDtos, "Get all studios success");
                return Ok(res);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpPost]
        [EndpointSummary("Create studio")]
        [EndpointDescription("Create studio from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<StudioResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<StudioResponse>>> CreateStudioAsync(CreateStudioRequest req)
        {
            try
            {
                var (studio, seats) = await _studioService.CreateAsync(req);
                var studioDto = await _studioMapper.ToDtoWithSeats(studio, seats.ToList());
                var res = new BaseResponseApi<StudioResponse>(studioDto, "Create studio success");
                return Ok(res);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update studio")]
        [EndpointDescription("Update studio from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<StudioResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<StudioResponse>>> UpdateStudioAsync(UpdateStudioRequest req, string id)
        {
            try
            {
                var (studio, seats) = await _studioService.UpdateAsync(req, id);
                var studioDto = await _studioMapper.ToDtoWithSeats(studio, seats.ToList());
                var res = new BaseResponseApi<StudioResponse>(studioDto, "Update studio success");
                return Ok(studioDto);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete studio")]
        [EndpointDescription("Delete studio from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<StudioResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<StudioResponse>>> DeleteStudioAsync(string id)
        {
            try
            {
                var studio = await _studioService.DeleteAsync(id);
                var studioDto = await _studioMapper.ToDto(studio);
                var res = new BaseResponseApi<StudioResponse>(studioDto, "Delete studio success");
                return Ok(studioDto);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }
    }
}