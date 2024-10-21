using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Booking;
using MovieApi.Responses.Booking;
using MovieApi.Services.BookingService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        private readonly BookingMapper _bookingMapper;

        public BookingController(IBookingService bookingService, BookingMapper bookingMapper)
        {
            _bookingService = bookingService;
            _bookingMapper = bookingMapper;
        }

        [HttpPost("admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BookingResponse), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BookingResponse>> BookingFromAdminAsync([FromForm] CreateBookingRequest req)
        {
            try
            {
                var (booking, seats) = await _bookingService.BookingFromAdminAsync(req);
                var bookingDto = await _bookingMapper.ToDto(booking, seats);
                return Ok(bookingDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("customer")]
        [Authorize(Roles = "customer")]
        [ProducesResponseType(type: typeof(BookingResponse), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BookingResponse>> BookingFromCustomerAsync([FromForm] CreateBookingRequest req)
        {
            try
            {
                var (booking, seats) = await _bookingService.BookingFromCustomerAsync(req);
                var bookingDto = await _bookingMapper.ToDto(booking, seats);
                return Ok(bookingDto);
            }   
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}