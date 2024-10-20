using MovieApi.Entities;
using MovieApi.Responses.Studio;
using MovieApi.Utilities;

namespace MovieApi.Mappers
{
    public class StudioMapper
    {
        private readonly SeatMapper _seatMapper;

        private readonly DateUtil _dateUtil;

        public StudioMapper(SeatMapper seatMapper, DateUtil dateUtil)
        {
            _seatMapper = seatMapper;
            _dateUtil = dateUtil;
        }

        public async Task<StudioResponse> ToDto(Studio studio)
        {
            return await Task.Run(() =>
            {
                return new StudioResponse
                {
                    Id = studio.Id,
                    Code = studio.Code,
                    Name = studio.Name,
                    TotalSeats = studio.TotalSeats,
                    CreatedAt = _dateUtil.GetDateToString(studio.CreatedAt),
                    UpdatedAt = _dateUtil.GetDateToString(studio.UpdatedAt),
                };
            });
        }

        public async Task<StudioResponse> ToDtoWithSeats(Studio studio, List<Seat> seats)
        {
            return await Task.Run(async () =>
            {
                return new StudioResponse
                {
                    Id = studio.Id,
                    Code = studio.Code,
                    Name = studio.Name,
                    TotalSeats = studio.TotalSeats,
                    CreatedAt = _dateUtil.GetDateToString(studio.CreatedAt),
                    UpdatedAt = _dateUtil.GetDateToString(studio.UpdatedAt),
                    AvailableSeats = seats.Where(x => x.IsAvailable == true).Count(),
                    ReservedSeats = seats.Where(x => x.IsAvailable == false).Count(),
                    Seats = await _seatMapper.ToDtos(seats)
                };
            });
        }

        public async Task<List<StudioResponse>> ToDtos(List<Studio> studios)
        {
            return await Task.Run(() =>
            {
                var listStudioResponse = new List<StudioResponse>();
                foreach (var studio in studios)
                {
                    listStudioResponse.Add(ToDto(studio).Result);
                }
                return listStudioResponse;
            });
        }
    }
}