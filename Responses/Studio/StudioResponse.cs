using System.Text.Json.Serialization;
using MovieApi.Responses.Seat;

namespace MovieApi.Responses.Studio
{
    public class StudioResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("total_seats")]
        public int? TotalSeats { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public string? UpdatedAt { get; set; }

        [JsonPropertyName("available_seats")]
        public int? AvailableSeats { get; set; }

        [JsonPropertyName("reserved_seats")]
        public int? ReservedSeats { get; set; }

        [JsonPropertyName("seats")]
        public List<SeatResponse>? Seats { get; set; }
    }
}