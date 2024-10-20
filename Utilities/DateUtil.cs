using System.Globalization;

namespace MovieApi.Utilities
{
    public class DateUtil
    {
        public DateTime GetDateNow() => DateTime.Now;

        public string GetDateTimeToString(DateTime? date) => date?.ToString("dd MMM yyyy HH:mm:ss");

        public string GetDateToString(DateTime? date) => date?.ToString("dddd, dd MMMM yyyy");

        public DateTime GetStringToDate(string date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public string GetTimeToString(TimeSpan? time)
        {
            return time?.ToString(@"hh\:mm");
        }

        public TimeSpan GetStringToTime(string time) 
        {
            return TimeSpan.ParseExact(time, @"hh\:mm", CultureInfo.InvariantCulture);
        }

        public bool IsDateInRangeOneWeek(DateTime? date) => date > DateTime.Now.AddDays(-7);
    }
}