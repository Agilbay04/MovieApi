namespace MovieApi.Requests.Booking
{
    public class ConfirmBookingRequest
    {
        public bool IsConfirmed { get; set; }

        public string? CancelReason { get; set; }
    }
}