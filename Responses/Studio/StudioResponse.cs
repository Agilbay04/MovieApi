using MovieApi.Responses.Seat;

namespace MovieApi.Responses.Studio
{
    public class StudioResponse
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? TotalSeats { get; set; }

        public int? AvailableSeats { get; set; }

        public int? ReservedSeats { get; set; }

        public List<SeatResponse>? Seats { get; set; }
    }
}