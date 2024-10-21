using System.Text.Json.Serialization;

namespace MovieApi.Responses.Seat
{
    public class SeatResponse
    {
        [JsonPropertyName("seat_number")]
        public string? SeatNumber { get; set; }

        [JsonPropertyName("is_available")]
        public bool IsAvailable { get; set; }
    }
}