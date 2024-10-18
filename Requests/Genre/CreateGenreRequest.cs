using System.ComponentModel.DataAnnotations;

namespace MovieApi.Requests.Genre
{
    public class CreateGenreRequest : BaseRequest
    {
        [Required]
        public string Name { get; set; }
    }
}