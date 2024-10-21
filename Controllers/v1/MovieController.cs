using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Mappers;
using MovieApi.Requests;
using MovieApi.Requests.Movie;
using MovieApi.Responses.Movie;
using MovieApi.Services.MovieService;

namespace MovieApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IMovieService _movieService;

        private readonly MovieMapper _movieMapper;

        public MovieController(
            AppDbContext context, 
            IMovieService movieService, 
            MovieMapper movieMapper)
        {
            _context = context;
            _movieService = movieService;
            _movieMapper = movieMapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(type: typeof(MovieResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<Movie>> FindMovieByIdAsync(string id)
        {
            try
            {
                var movie = await _movieService.FindByIdAsync(id);
                var movieDto = await _movieMapper.ToDto(movie);
                return Ok(movieDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(type: typeof(List<MovieResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<List<MovieResponse>>> FindAllMoviesAsync()
        {
            var movies = await _movieService.FindAllAsync();
            var movieDtos = await _movieMapper.ToDtos(movies.ToList());
            return Ok(movieDtos);
        }

        [HttpPost]
        [ProducesResponseType(type: typeof(MovieResponse), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<MovieResponse>> CreateMovieAsync([FromForm] CreateMovieRequest req)
        {
            try
            {
                var movie = await _movieService.CreateAsync(req);
                var movieDto = await _movieMapper.ToDto(movie);
                return Ok(movieDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(type: typeof(MovieResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<Movie>> UpdateMovieAsync([FromForm] UpdateMovieRequest req, string id)
        {
            try 
            {
                var movie = await _movieService.UpdateAsync(req, id);
                var movieDto = await _movieMapper.ToDto(movie);
                return Ok(movieDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<MovieResponse>> DeleteMovieAsync(string id)
        {
            try
            {
                var movie = await _movieService.DeleteAsync(id);
                var movieDto = await _movieMapper.ToDto(movie);
                return Ok(movieDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}