using MovieApi.Entities;
using MovieApi.Responses.Seat;

namespace MovieApi.Mappers
{
    public class SeatMapper
    {
        public async Task<SeatResponse> ToDto(Seat seat)
        {
            return await Task.Run(() =>
            {
                return new SeatResponse
                {
                    SeatNumber = seat.SeatNumber
                };
            });
        }

        public async Task<List<SeatResponse>> ToDtos(List<Seat> seats)
        {
            return await Task.Run(() =>
            {
                var listSeatResponse = new List<SeatResponse>();
                foreach (var seat in seats)
                {
                    listSeatResponse.Add(ToDto(seat).Result);
                }
                return listSeatResponse;
            });
        }
    }
}