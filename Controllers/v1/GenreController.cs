using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Mappers;
using MovieApi.Requests.Genre;
using MovieApi.Responses.Genre;
using MovieApi.Services.GenreService;

namespace MovieApi.Controllers.v1
{
    [ApiController]
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
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
        [ProducesResponseType(type: typeof(GenreResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<GenreResponse>> FindGenreByIdAsync(string id)
        {
            try
            {
                var genre = await _genreService.FindByIdAsync(id);
                var genreDto = await _genreMapper.ToDto(genre);
                return Ok(genreDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(type: typeof(List<GenreResponse>), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GenreResponse>>> FindAllGenresAsync()
        {   
            try
            {
                var genres = await _genreService.FindAllAsync();
                var genreDtos = await _genreMapper.ToDtos(genres.ToList());
                return Ok(genreDtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(type: typeof(GenreResponse), statusCode: StatusCodes.Status201Created)]
        public async Task<ActionResult<GenreResponse>> CreateGenreAsync(CreateGenreRequest req)
        {
            try
            {
                var genre = await _genreService.CreateAsync(req);
                var genreDto = await _genreMapper.ToDto(genre);
                return Ok(genreDto);
            }
            catch(BadHttpRequestException ex)
            {
                throw new BadHttpRequestException(ex.Message);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(type: typeof(GenreResponse), statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<GenreResponse>> UpdateGenreAsync(UpdateGenreRequest req, string id)
        {
            try 
            {
                var genre = await _genreService.UpdateAsync(req, id);
                var genreDto = await _genreMapper.ToDto(genre);
                return Ok(genreDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<GenreResponse>> DeleteGenreAsync(string id)
        {
            try
            {
                var genre = await _genreService.DeleteAsync(id);
                var genreDto = await _genreMapper.ToDto(genre);
                return Ok(genreDto);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}