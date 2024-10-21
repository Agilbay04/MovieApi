namespace MovieApi.Requests.Movie
{
    public class UpdateMovieRequest
    {
        public string Title { get; set; }

        public IFormFile? Poster { get; set; }
        
        public string Description { get; set; }
        
        public int Duration { get; set; }

        public string ReleaseDate { get; set; }
        
        public bool IsPublished { get; set; }
        
        public List<string>? ListOfGenres { get; set; }
    }
}