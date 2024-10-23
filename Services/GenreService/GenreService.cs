using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Exceptions;
using MovieApi.Requests.Genre;
using MovieApi.Services.UserService;

namespace MovieApi.Services.GenreService
{
    public class GenreService : IGenreService
    {
        private readonly AppDbContext _context;

        private readonly IUserService _userService;

        public GenreService(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Genre> FindByIdAsync(string id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id && 
                g.Deleted == false) ?? 
                throw new NotFoundException("Genre not found");
            
            return genre;
        }

        public async Task<IEnumerable<Genre>> FindAllAsync()
        {
            return await _context.Genres
                .Where(g => g.Deleted == false)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
        }

        public async Task<Genre> CreateAsync(CreateGenreRequest req)
        {
            var isNameExists = await _context.Genres
                .FirstOrDefaultAsync(g => g.Name.ToLower() == req.Name.ToLower() && 
                g.Deleted == false);

            if (isNameExists != null)
                throw new BadRequestException("Genre name already exists");

            var genre = new Genre
            {
                Name = req.Name,
                CreatedBy = _userService.GetUserId(),
            };
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<Genre> UpdateAsync(UpdateGenreRequest req, string id)
        {
            var genre = await FindByIdAsync(id);

            genre.Name = req.Name;
            genre.UpdatedAt = DateTime.Now;
            genre.UpdatedBy = _userService.GetUserId();

            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<Genre> DeleteAsync(string id)
        {
            var genreInUsed = await _context.MovieGenres
                .Where(mg => mg.GenreId == id && mg.Deleted == false)
                .FirstOrDefaultAsync();
            
            if (genreInUsed != null)
                throw new BadRequestException("Genre in used");

            var genre = await FindByIdAsync(id);

            genre.Deleted = true;
            genre.UpdatedAt = DateTime.Now;;
            genre.UpdatedBy = _userService.GetUserId();

            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
            return genre;
        }
        
    }
}