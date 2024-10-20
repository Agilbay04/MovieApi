using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Showtime;
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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(ShowtimeResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<ShowtimeResponse>> FindShowtimeByIdAsync(string id)
        {
            try
            {
                var showtime = await _showtimeService.FindByIdAsync(id);
                var showtimeDto = await _showtimeMapper.ToDto(showtime);
                return Ok(showtimeDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(List<ShowtimeResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ShowtimeResponse>>> FindAllShowtimesAsync()
        {
            try
            {
                var showtimes = await _showtimeService.FindAllAsync();
                var showtimesDto = await _showtimeMapper.ToDtos(showtimes.ToList());
                return Ok(showtimesDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(ShowtimeResponse), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<ShowtimeResponse>> CreateShowtimeAsync(CreateShowtimeRequest req)
        {
            try
            {
                var showtime = await _showtimeService.CreateAsync(req);
                var showtimeDto = await _showtimeMapper.ToDto(showtime);
                return Ok(showtimeDto);
            }
            catch (BadHttpRequestException ex)
            {
                throw new BadHttpRequestException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(ShowtimeResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<ShowtimeResponse>> UpdateShowtimeAsync(UpdateShowtimeRequest req, string id)
        {
            try
            {
                var showtime = await _showtimeService.UpdateAsync(req, id);
                var showtimeDto = await _showtimeMapper.ToDto(showtime);
                return Ok(showtimeDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(ShowtimeResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<ShowtimeResponse>> DeleteShowtimeAsync(string id)
        {
            try
            {
                var showtime = await _showtimeService.DeleteAsync(id);
                var showtimeDto = await _showtimeMapper.ToDto(showtime);
                return Ok(showtimeDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}