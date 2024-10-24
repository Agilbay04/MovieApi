using MovieApi.Entities;
using MovieApi.Responses.Price;
using MovieApi.Utilities;

namespace MovieApi.Mappers
{
    public class PriceMapper
    {
        private readonly DateUtil _dateUtil;

        private readonly PriceUtil _priceUtil;

        public PriceMapper(DateUtil dateUtil, PriceUtil priceUtil)
        {
            _dateUtil = dateUtil;
            _priceUtil = priceUtil;
        }

        public async Task<PriceResponse> ToDto(Price price)
        {
            return await Task.Run(() => { 
                return new PriceResponse
                {
                    Id = price.Id,
                    Code = price.Code,
                    Name = price.Name,
                    Description = price.Description,
                    PriceValue = _priceUtil.GetIDRCurrency(price.PriceValue),
                    UpdatedAt = _dateUtil.GetDateTimeToString(price.UpdatedAt),
                    CreatedAt = _dateUtil.GetDateTimeToString(price.CreatedAt)
                };
            });
        }

        public async Task<List<PriceResponse>> ToDtos(List<Price> prices)
        {
            return await Task.Run(() => { 
                var listPriceResponse = new List<PriceResponse>();
                foreach (var price in prices)
                {
                    listPriceResponse.Add(ToDto(price).Result);
                }
                return listPriceResponse;
            });
        }        
    }
}