using System.Text.Json.Serialization;

namespace MovieApi.Responses.Booking
{
    public class BookingResponse
    {
        [JsonPropertyName("booking_cod")]
        public string? BookingCode { get; set; }

        [JsonPropertyName("customer_name")]
        public string? CustomerName { get; set; }

        [JsonPropertyName("movie_title")]
        public string? MovieTitle { get; set; }

        [JsonPropertyName("start_time")]
        public string? StartTime { get; set; }

        [JsonPropertyName("play_date")]
        public string? PlayDate { get; set; }

        [JsonPropertyName("booking_date")]
        public string? BookingDate { get; set; }

        [JsonPropertyName("payment_type")]
        public string? PaymentType { get; set; }

        [JsonPropertyName("payment_status")]
        public string? PaymentStatus { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }

        [JsonPropertyName("price_value")]
        public string? PriceValue { get; set; }

        [JsonPropertyName("total_price")]
        public string? TotalPrice { get; set; }

        [JsonPropertyName("seats")]
        public List<string>? Seats { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("created_by")]
        public string? CreatedBy { get; set; }

        [JsonPropertyName("updated_at")]
        public string? UpdatedAt { get; set; }

        [JsonPropertyName("updated_by")]
        public string? UpdatedBy { get; set; }
    }
}