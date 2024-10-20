using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Price;
using MovieApi.Responses.Price;
using MovieApi.Services.PriceService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
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
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(PriceResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<PriceResponse>> FindPriceByIdAsync(string id)
        {
            try
            {
                var price = await _priceService.FindByIdAsync(id);
                var priceDto = await _priceMapper.ToDto(price);
                return Ok(priceDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(List<PriceResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<PriceResponse>> FindAllPricesAsync()
        {
            try
            {
                var prices = await _priceService.FindAllAsync();
                var pricesDto = await _priceMapper.ToDtos(prices.ToList());
                return Ok(pricesDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(PriceResponse), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<PriceResponse>> CreatePriceAsync(CreatePriceRequest req)
        {
            try
            {
                var price = await _priceService.CreateAsync(req);
                var priceDto = await _priceMapper.ToDto(price);
                return Ok(priceDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(PriceResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<PriceResponse>> UpdatePriceAsync(UpdatePriceRequest req, string id)
        {
            try
            {
                var price = await _priceService.UpdateAsync(req, id);
                var priceDto = await _priceMapper.ToDto(price);
                return Ok(priceDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(PriceResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<PriceResponse>> DeletePriceAsync(string id)
        {
            try
            {
                var price = await _priceService.DeleteAsync(id);
                var priceDto = await _priceMapper.ToDto(price);
                return Ok(priceDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}