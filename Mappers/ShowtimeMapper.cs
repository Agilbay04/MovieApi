using MovieApi.Entities;
using MovieApi.Responses.Showtime;
using MovieApi.Utilities;

namespace MovieApi.Mappers
{
    public class ShowtimeMapper
    {
        private readonly DateUtil _dateUtil;

        private readonly PriceUtil _priceUtil;

        public ShowtimeMapper(DateUtil dateUtil, PriceUtil priceUtil)
        {
            _dateUtil = dateUtil;
            _priceUtil = priceUtil;
        }

        public async Task<ShowtimeResponse> ToDto(Showtime showtime)
        {
            return await Task.Run(() =>
            {
                return new ShowtimeResponse
                {
                    Id = showtime.Id,
                    MovieId = showtime.MovieId,
                    MovieTitle = showtime.Movie?.Title,
                    StudioId = showtime.StudioId,
                    StudioCode = showtime.Studio?.Code,
                    StudioName = showtime.Studio?.Name,
                    PriceCode = showtime.Price?.Code,
                    Price = _priceUtil.GetIDRCurrency(showtime.Price?.PriceValue ?? 0),
                    StartTime = _dateUtil.GetTimeToString(showtime.StartTime),
                    PlayDate = _dateUtil.GetDateToString(showtime.PlayDate),
                    CreatedAt = _dateUtil.GetDateTimeToString(showtime.CreatedAt),
                    UpdatedAt = _dateUtil.GetDateTimeToString(showtime.UpdatedAt)
                };
            });
        }

        public async Task<List<ShowtimeResponse>> ToDtos(List<Showtime> showtimes)
        {
            return await Task.Run(() =>
            {
                var listShowtimeResponse = new List<ShowtimeResponse>();
                foreach (var showtime in showtimes)
                {
                    listShowtimeResponse.Add(ToDto(showtime).Result);
                }
                return listShowtimeResponse;
            });
        }
    }
}