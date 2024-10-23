using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Booking;
using MovieApi.Responses;
using MovieApi.Responses.Booking;
using MovieApi.Services.BookingService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
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
        [ProducesResponseType(type: typeof(BaseResponseApi<BookingResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<BookingResponse>>> GetBookingByIdAsync(string id)
        {
            var (booking, seats, user) = await _bookingService.FindBookingByIdAsync(id);
            var bookingDto = await _bookingMapper.ToDto(booking, seats, user);
            var res = new BaseResponseApi<BookingResponse>(bookingDto, "Get booking successful");
            return Ok(res);
        }

        [HttpGet("code/{code}")]
        [EndpointSummary("Get booking by code")]
        [EndpointDescription("Get booking by code from customer or admin")]
        [Authorize]
        [ProducesResponseType(type: typeof(BaseResponseApi<BookingResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<BookingResponse>>> GetBookingByCodeAsync(string code)
        {
            var (booking, seats, user) = await _bookingService.FindBookingByCodeAsync(code);
            var bookingDto = await _bookingMapper.ToDto(booking, seats, user);
            var res = new BaseResponseApi<BookingResponse>(bookingDto, "Get booking successful");
            return Ok(res);
        }

        [HttpGet]
        [EndpointSummary("Get all booking")]
        [EndpointDescription("Get all booking from user role admin")]
        [Authorize]
        [ProducesResponseType(type: typeof(BaseResponseApi<BookingResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<List<BookingResponse>>>> GetAllBookingAsync()
        {
            var bookings = await _bookingService.FindAllBookingAsync();
            var bookingDtos = await _bookingMapper.ToDtos(bookings);
            var res = new BaseResponseApi<List<BookingResponse>>(bookingDtos, "Get all booking successful");
            return Ok(res);
        }

        [HttpPost("admin")]
        [EndpointSummary("Create booking")]
        [EndpointDescription("Create booking from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<BookingResponse>), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BaseResponseApi<BookingResponse>>> BookingFromAdminAsync([FromForm] CreateBookingRequest req)
        {
            var (booking, seats, user) = await _bookingService.BookingFromAdminAsync(req);
            var bookingDto = await _bookingMapper.ToDto(booking, seats, user);
            var res = new BaseResponseApi<BookingResponse>(bookingDto, "Create booking successful");
            return Ok(res);
        }

        [HttpPost("customer")]
        [EndpointSummary("Create booking")]
        [EndpointDescription("Create booking from customer")]
        [Authorize(Roles = "customer")]
        [ProducesResponseType(type: typeof(BaseResponseApi<BookingResponse>), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BaseResponseApi<BookingResponse>>> BookingFromCustomerAsync([FromForm] CreateBookingRequest req)
        {
            var (booking, seats, user) = await _bookingService.BookingFromCustomerAsync(req);
            var bookingDto = await _bookingMapper.ToDto(booking, seats, user);
            var res = new BaseResponseApi<BookingResponse>(bookingDto, "Create booking successful");
            return Ok(res);
        }

        [HttpPut("admin/confirm/{bookingCode}")]
        [EndpointSummary("Confirm booking")]
        [EndpointDescription("Confirm booking from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<BookingResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<BookingResponse>>> ConfirmBookingAsync([FromForm] ConfirmBookingRequest req, string bookingCode)
        {
            var (booking, seats, user) = await _bookingService.ConfirmBookingAsync(req, bookingCode);
            var bookingDto = await _bookingMapper.ToDto(booking, seats, user);
            var res = new BaseResponseApi<BookingResponse>(bookingDto, "Confirm booking successful");
            return Ok(res);
        }

        [HttpPut("customer/upload/{bookingCode}")]
        [EndpointSummary("Upload payment proof")]
        [EndpointDescription("Upload payment proof from customer")]
        [Authorize(Roles = "customer")]
        [ProducesResponseType(type: typeof(BaseResponseApi<BookingResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<BookingResponse>>> UploadPaymentProofAsync(IFormFile paymentProof, string bookingCode)
        {
            var (booking, seats, user) = await _bookingService.UploadPaymentProofAsync(paymentProof, bookingCode);
            var bookingDto = await _bookingMapper.ToDto(booking, seats, user);
            var res = new BaseResponseApi<BookingResponse>(bookingDto, "Upload payment proof successful");
            return Ok(res);
        }

    }
}