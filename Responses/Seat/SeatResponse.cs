using System.Text.Json.Serialization;

namespace MovieApi.Responses.Seat
{
    public class SeatResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("seat_number")]
        public string? SeatNumber { get; set; }

        [JsonPropertyName("row")]
        public string? Row { get; set; }

        [JsonPropertyName("column")]
        public int? Column { get; set; }

        [JsonPropertyName("is_available")]
        public bool IsAvailable { get; set; }
    }
}