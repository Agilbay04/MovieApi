using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Genre;
using MovieApi.Responses;
using MovieApi.Responses.Genre;
using MovieApi.Services.GenreService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/v1/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        
        private readonly GenreMapper _genreMapper;

        public GenreController(IGenreService genreService, GenreMapper genreMapper)
        {
            _genreService = genreService;
            _genreMapper = genreMapper;
        }

        [HttpGet("{id}")]
        [EndpointSummary("Get genre by id")]
        [EndpointDescription("Get genre by id from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<GenreResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<GenreResponse>>> FindGenreByIdAsync(string id)
        {
            var genre = await _genreService.FindByIdAsync(id);
            var genreDto = await _genreMapper.ToDto(genre);
            var res = new BaseResponseApi<GenreResponse>(genreDto, "Get genre successful");
            return Ok(res);
        }

        [HttpGet]
        [EndpointSummary("Get all genres")]
        [EndpointDescription("Get all genres from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<List<GenreResponse>>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BaseResponseApi<GenreResponse>>>> FindAllGenresAsync()
        {
            var genres = await _genreService.FindAllAsync();
            var genreDtos = await _genreMapper.ToDtos(genres.ToList());
            var res = new BaseResponseApi<List<GenreResponse>>(genreDtos, "Get all genre successful");
            return Ok(res);
        }

        [HttpPost]
        [EndpointSummary("Create genre")]
        [EndpointDescription("Create genre from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<GenreResponse>), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<BaseResponseApi<GenreResponse>>> CreateGenreAsync(CreateGenreRequest req)
        {
            var genre = await _genreService.CreateAsync(req);
            var genreDto = await _genreMapper.ToDto(genre);
            var res = new BaseResponseApi<GenreResponse>(genreDto, "Create genre successful");
            return Ok(res);
        }

        [HttpPut("{id}")]
        [EndpointSummary("Update genre")]
        [EndpointDescription("Update genre from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<GenreResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<GenreResponse>>> UpdateGenreAsync(UpdateGenreRequest req, string id)
        {
            var genre = await _genreService.UpdateAsync(req, id);
            var genreDto = await _genreMapper.ToDto(genre);
            var res = new BaseResponseApi<GenreResponse>(genreDto, "Update genre successful");
            return Ok(res);
        }

        [HttpDelete("{id}")]
        [EndpointSummary("Delete genre")]
        [EndpointDescription("Delete genre from admin")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(type: typeof(BaseResponseApi<GenreResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponseApi<GenreResponse>>> DeleteGenreAsync(string id)
        {
            var genre = await _genreService.DeleteAsync(id);
            var genreDto = await _genreMapper.ToDto(genre);
            var res = new BaseResponseApi<GenreResponse>(genreDto, "Delete genre successful");
            return Ok(res);
        }

    }
}