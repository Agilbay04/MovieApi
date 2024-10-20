using MovieApi.Responses.Seat;

namespace MovieApi.Responses.Studio
{
    public class StudioResponse
    {
        public string? Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? TotalSeats { get; set; }

        public string? CreatedAt { get; set; }

        public string? UpdatedAt { get; set; }

        public int? AvailableSeats { get; set; }

        public int? ReservedSeats { get; set; }

        public List<SeatResponse>? Seats { get; set; }
    }
}