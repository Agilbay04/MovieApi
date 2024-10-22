using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Showtime;
using MovieApi.Responses;
using MovieApi.Responses.Showtime;
using MovieApi.Services.ShowtimeService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowtimeController : ControllerBase
    {
        private readonly ShowtimeMapper _showtimeMapper;

        private readonly IShowtimeService _showtimeService;

        public ShowtimeController(ShowtimeMapper showtimeMapper, IShowtimeService showtimeService)
        {
            _showtimeMapper = showtimeMapper;
            _showtimeService = showtimeService;
        }

        [HttpGet("{id}")]
        [EndpointSummary("Find showtime by id")]
        [EndpointDescription("Find showtime by id from admin or customer")]
        [Authorize]
        [ProducesResponseType(type: typeof(BaseResponseApi<ShowtimeResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<ShowtimeResponse>>> FindShowtimeByIdAsync(string id)
        {
            try
            {
                var (showtime, seats) = await _showtimeService.FindByIdAsync(id);
                var showtimeDto = await _showtimeMapper.ToDtoDetail(showtime, seats);
                var res = new BaseResponseApi<ShowtimeResponse>(showtimeDto, "Find showtime by id successful");
                return Ok(res);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpGet]
        [EndpointSummary("Find all showtimes")]
        [EndpointDescription("Find all showtimes from admin or customer")]
        [Authorize]
        [ProducesResponseType(type: typeof(BaseResponseApi<List<ShowtimeResponse>>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<List<ShowtimeResponse>>>> FindAllShowtimesAsync()
        {
            try
            {
                var showtimes = await _showtimeService.FindAllAsync();
                var showtimesDto = await _showtimeMapper.ToDtos(showtimes.ToList());
                var res = new BaseResponseApi<List<ShowtimeResponse>>(showtimesDto, "Find all showtimes successful");
                return Ok(res);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpPost]
        [EndpointSummary("Create showtime")]
        [EndpointDescription("Create showtime from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<ShowtimeResponse>), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BaseResponseApi<ShowtimeResponse>>> CreateShowtimeAsync(CreateShowtimeRequest req)
        {
            try
            {
                var showtime = await _showtimeService.CreateAsync(req);
                var showtimeDto = await _showtimeMapper.ToDto(showtime);
                var res = new BaseResponseApi<ShowtimeResponse>(showtimeDto, "Create showtime successful");
                return Ok(res);
            }
            catch (BadHttpRequestException ex)
            {
                throw new BadHttpRequestException(ex.Message);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update showtime")]
        [EndpointDescription("Update showtime from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<ShowtimeResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<ShowtimeResponse>>> UpdateShowtimeAsync(UpdateShowtimeRequest req, string id)
        {
            try
            {
                var showtime = await _showtimeService.UpdateAsync(req, id);
                var showtimeDto = await _showtimeMapper.ToDto(showtime);
                var res = new BaseResponseApi<ShowtimeResponse>(showtimeDto, "Update showtime successful");
                return Ok(res);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete showtime")]
        [EndpointDescription("Delete showtime from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponseApi<ShowtimeResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<ShowtimeResponse>>> DeleteShowtimeAsync(string id)
        {
            try
            {
                var showtime = await _showtimeService.DeleteAsync(id);
                var showtimeDto = await _showtimeMapper.ToDto(showtime);
                var res = new BaseResponseApi<ShowtimeResponse>(showtimeDto, "Delete showtime successfull");
                return Ok(res);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }
    }
}