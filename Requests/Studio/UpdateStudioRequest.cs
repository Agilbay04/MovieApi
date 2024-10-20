namespace MovieApi.Requests.Studio
{
    public class UpdateStudioRequest
    {
        public string Name { get; set; }

        public string Facility { get; set; }

        public int TotalSeats { get; set; }

        public int RowPerSeats { get; set; }
    }
}