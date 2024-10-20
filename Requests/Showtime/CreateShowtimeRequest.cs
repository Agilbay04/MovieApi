using System.ComponentModel.DataAnnotations;

namespace MovieApi.Requests.Showtime
{
    public class CreateShowtimeRequest
    {
        [Required]
        public string MovieId { get; set; }
        
        [Required]
        public string StudioId { get; set; }
        
        [Required]
        public string StartTime { get; set; }
        
        [Required]
        public string PlayDate { get; set; }
    }
}