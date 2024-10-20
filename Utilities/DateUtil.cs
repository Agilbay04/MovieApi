namespace MovieApi.Utilities
{
    public class DateUtil
    {
        public DateTime GetDateNow() => DateTime.Now;

        public string GetDateToString(DateTime? date) => date?.ToString("dd MMM yyyy HH:mm:ss");
    }
}