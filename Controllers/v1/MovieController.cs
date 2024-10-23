using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Entities;
using MovieApi.Mappers;
using MovieApi.Requests;
using MovieApi.Requests.Movie;
using MovieApi.Responses;
using MovieApi.Responses.Movie;
using MovieApi.Services.MovieService;

namespace MovieApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        private readonly MovieMapper _movieMapper;

        public MovieController(
            IMovieService movieService, 
            MovieMapper movieMapper)
        {
            _movieService = movieService;
            _movieMapper = movieMapper;
        }

        [HttpGet("{id}")]
        [EndpointSummary("Find movie by id")]
        [EndpointDescription("Find movie by id from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<MovieResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<Movie>>> FindMovieByIdAsync(string id)
        {
            try
            {
                var (movie, genres) = await _movieService.FindByIdAsync(id);
                var movieDto = await _movieMapper.ToDto(movie, genres);
                var res = new BaseResponseApi<MovieResponse>(movieDto, "Get movie successful");
                return Ok(res);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpGet]
        [EndpointSummary("Find all movies")]
        [EndpointDescription("Find all movies from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<List<MovieResponse>>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<List<MovieResponse>>>> FindAllMoviesAsync()
        {
            try
            {
                var movies = await _movieService.FindAllAsync();
                var res = new BaseResponseApi<List<MovieResponse>>(movies, "Get all movie successful");
                return Ok(res);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpPost]
        [EndpointSummary("Create movie")]
        [EndpointDescription("Create movie from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<MovieResponse>), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BaseResponseApi<MovieResponse>>> CreateMovieAsync([FromForm] CreateMovieRequest req)
        {
            try
            {
                var (movie, seats) = await _movieService.CreateAsync(req);
                var movieDto = await _movieMapper.ToDto(movie, seats);
                var res = new BaseResponseApi<MovieResponse>(movieDto, "Create movie successful");
                return Ok(res);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update movie")]
        [EndpointDescription("Update movie from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<MovieResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<Movie>>> UpdateMovieAsync([FromForm] UpdateMovieRequest req, string id)
        {
            try 
            {
                var (movie, seats) = await _movieService.UpdateAsync(req, id);
                var movieDto = await _movieMapper.ToDto(movie, seats);
                var res = new BaseResponseApi<MovieResponse>(movieDto, "Update movie successful");
                return Ok(res);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete movie")]
        [EndpointDescription("Delete movie from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<MovieResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<MovieResponse>>> DeleteMovieAsync(string id)
        {
            try
            {
                var (movie, seats) = await _movieService.DeleteAsync(id);
                var movieDto = await _movieMapper.ToDto(movie, seats);
                var res = new BaseResponseApi<MovieResponse>(movieDto, "Delete movie successful");
                return Ok(res);
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw ex;
            }
        }

    }
}