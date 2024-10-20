namespace MovieApi.Requests.Studio
{
    public class CreateStudioRequest
    {
        public string Name { get; set; }

        public string Facility { get; set; }

        public int TotalSeats { get; set; }

        public int RowPerSeats { get; set; }
    }
}