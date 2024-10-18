using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MovieApi.Constants;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Requests.Genre;

namespace MovieApi.Services.GenreService
{
    public class GenreService : IGenreService
    {
        private readonly AppDbContext _context;

        public GenreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> FindByIdAsync(string id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id && 
                g.Deleted == (int)AppConstant.StatusDelete.NotDeleted) ?? 
                throw new Exception("Genre not found");
            
            return genre;
        }

        public async Task<IEnumerable<Genre>> FindAllAsync()
        {
            return await _context.Genres
                .Where(g => g.Deleted == (int)AppConstant.StatusDelete.NotDeleted)
                .ToListAsync();
        }

        public async Task<Genre> CreateAsync(CreateGenreRequest req)
        {
            var isNameExists = await _context.Genres
                .FirstOrDefaultAsync(g => g.Name.ToLower() == req.Name.ToLower() && 
                g.Deleted == (int)AppConstant.StatusDelete.NotDeleted);

            if (isNameExists != null)
                throw new BadHttpRequestException("Genre name already exists");

            var genre = new Genre
            {
                Name = req.Name,
                CreatedBy = req.UserId
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
            genre.UpdatedBy = req.UserId;

            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<Genre> DeleteAsync(string id)
        {
            var genreInUsed = await _context.MovieGenres
                .Where(mg => mg.GenreId == id && mg.Deleted == (int)AppConstant.StatusDelete.NotDeleted)
                .FirstOrDefaultAsync();
            
            if (genreInUsed != null)
                throw new Exception("Genre in used");

            var genre = await FindByIdAsync(id);

            genre.Deleted = (int)AppConstant.StatusDelete.Deleted;
            genre.UpdatedAt = DateTime.Now;;

            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
            return genre;
        }
        
    }
}