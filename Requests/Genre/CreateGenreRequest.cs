using System.ComponentModel.DataAnnotations;

namespace MovieApi.Requests.Genre
{
    public class CreateGenreRequest
    {
        [Required]
        public string Name { get; set; }
    }
}