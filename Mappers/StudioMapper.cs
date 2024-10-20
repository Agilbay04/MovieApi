using MovieApi.Entities;
using MovieApi.Responses.Studio;

namespace MovieApi.Mappers
{
    public class StudioMapper
    {
        private readonly SeatMapper _seatMapper;

        public StudioMapper(SeatMapper seatMapper)
        {
            _seatMapper = seatMapper;
        }

        public async Task<StudioResponse> ToDto(Studio studio, List<Seat> seats)
        {
            return await Task.Run(async () =>
            {
                return new StudioResponse
                {
                    Code = studio.Code,
                    Name = studio.Name,
                    TotalSeats = studio.TotalSeats,
                    AvailableSeats = seats.Where(x => x.IsAvailable == true).Count(),
                    ReservedSeats = seats.Where(x => x.IsAvailable == false).Count(),
                    Seats = await _seatMapper.ToDtos(seats)
                };
            });
        }
    }
}