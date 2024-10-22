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

        [HttpGet("{id}")]
        [EndpointSummary("Get booking by id")]
        [EndpointDescription("Get booking by id from customer or admin")]
        [Authorize]
        [ProducesResponseType(type: typeof(BookingResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BookingResponse>> GetBookingByIdAsync(string id)
        {
            try
            {
                var (booking, seats, user) = await _bookingService.FindBookingByIdAsync(id);
                var bookingDto = await _bookingMapper.ToDto(booking, seats, user);
                return Ok(bookingDto);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("code/{code}")]
        [EndpointSummary("Get booking by code")]
        [EndpointDescription("Get booking by code from customer or admin")]
        [Authorize]
        [ProducesResponseType(type: typeof(BookingResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BookingResponse>> GetBookingByCodeAsync(string code)
        {
            try
            {
                var (booking, seats, user) = await _bookingService.FindBookingByCodeAsync(code);
                var bookingDto = await _bookingMapper.ToDto(booking, seats, user);
                return Ok(bookingDto);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [EndpointSummary("Get all booking")]
        [EndpointDescription("Get all booking from user role admin")]
        [Authorize]
        [ProducesResponseType(type: typeof(BookingResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BookingResponse>> GetAllBookingAsync()
        {
            try
            {
                var bookings = await _bookingService.FindAllBookingAsync();
                var bookingDtos = await _bookingMapper.ToDtos(bookings);
                return Ok(bookingDtos);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("admin")]
        [EndpointSummary("Create booking")]
        [EndpointDescription("Create booking from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BookingResponse), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BookingResponse>> BookingFromAdminAsync([FromForm] CreateBookingRequest req)
        {
            try
            {
                var (booking, seats, user) = await _bookingService.BookingFromAdminAsync(req);
                var bookingDto = await _bookingMapper.ToDto(booking, seats, user);
                return Ok(bookingDto);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("customer")]
        [EndpointSummary("Create booking")]
        [EndpointDescription("Create booking from customer")]
        [Authorize(Roles = "customer")]
        [ProducesResponseType(type: typeof(BookingResponse), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BookingResponse>> BookingFromCustomerAsync([FromForm] CreateBookingRequest req)
        {
            try
            {
                var (booking, seats, user) = await _bookingService.BookingFromCustomerAsync(req);
                var bookingDto = await _bookingMapper.ToDto(booking, seats, user);
                return Ok(bookingDto);
            }   
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("admin/confirm/{bookingCode}")]
        [EndpointSummary("Confirm booking")]
        [EndpointDescription("Confirm booking from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BookingResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BookingResponse>> ConfirmBookingAsync([FromForm] ConfirmBookingRequest req, string bookingCode)
        {
            try
            {
                var (booking, seats, user) = await _bookingService.ConfirmBookingAsync(req, bookingCode);
                var bookingDto = await _bookingMapper.ToDto(booking, seats, user);
                return Ok(bookingDto);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("customer/upload/{bookingCode}")]
        [EndpointSummary("Upload payment proof")]
        [EndpointDescription("Upload payment proof from customer")]
        [Authorize(Roles = "customer")]
        [ProducesResponseType(type: typeof(BookingResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BookingResponse>> UploadPaymentProofAsync(IFormFile paymentProof, string bookingCode)
        {
            try
            {
                var (booking, seats, user) = await _bookingService.UploadPaymentProofAsync(paymentProof, bookingCode);
                var bookingDto = await _bookingMapper.ToDto(booking, seats, user);
                return Ok(bookingDto);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw new Exception(ex.Message);
            }
        }

    }
}