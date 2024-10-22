using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Price;
using MovieApi.Responses;
using MovieApi.Responses.Price;
using MovieApi.Services.PriceService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PriceController : ControllerBase
    {
        private readonly PriceMapper _priceMapper;

        private readonly IPriceService _priceService;

        public PriceController(PriceMapper priceMapper, IPriceService priceService)
        {
            _priceMapper = priceMapper;
            _priceService = priceService;
        }

        [HttpGet("{id}")]
        [EndpointSummary("Find price by id")]
        [EndpointDescription("Find price by id from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<PriceResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<PriceResponse>>> FindPriceByIdAsync(string id)
        {
            try
            {
                var price = await _priceService.FindByIdAsync(id);
                var priceDto = await _priceMapper.ToDto(price);
                var res = new BaseResponseApi<PriceResponse>(priceDto, "Find price by id successful");
                return Ok(res);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpGet]
        [EndpointSummary("Find all prices")]
        [EndpointDescription("Find all prices from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<List<PriceResponse>>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<List<PriceResponse>>>> FindAllPricesAsync()
        {
            try
            {
                var prices = await _priceService.FindAllAsync();
                var pricesDto = await _priceMapper.ToDtos(prices);
                var res = new BaseResponseApi<List<PriceResponse>>(pricesDto, "Find all prices successful");
                return Ok(res);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpPost]
        [EndpointSummary("Create price")]
        [EndpointDescription("Create price from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<PriceResponse>), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BaseResponseApi<PriceResponse>>> CreatePriceAsync(CreatePriceRequest req)
        {
            try
            {
                var price = await _priceService.CreateAsync(req);
                var priceDto = await _priceMapper.ToDto(price);
                var res = new BaseResponseApi<PriceResponse>(priceDto, "Create price successful");
                return Ok(res);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update price")]
        [EndpointDescription("Update price from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<PriceResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<PriceResponse>>> UpdatePriceAsync(UpdatePriceRequest req, string id)
        {
            try
            {
                var price = await _priceService.UpdateAsync(req, id);
                var priceDto = await _priceMapper.ToDto(price);
                var res = new BaseResponseApi<PriceResponse>(priceDto, "Update price successful");
                return Ok(res);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete price")]
        [EndpointDescription("Delete price from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponseApi<PriceResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<PriceResponse>>> DeletePriceAsync(string id)
        {
            try
            {
                var price = await _priceService.DeleteAsync(id);
                var priceDto = await _priceMapper.ToDto(price);
                var res = new BaseResponseApi<PriceResponse>(priceDto, "Delete price successful");
                return Ok(res);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }
    }
}