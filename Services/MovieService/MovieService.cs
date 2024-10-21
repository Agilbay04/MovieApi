using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Requests;
using MovieApi.Requests.Movie;
using MovieApi.Services.UploadService;
using MovieApi.Utilities;

namespace MovieApi.Services.MovieService
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;

        private readonly DateUtil _dateUtil;

        private readonly IUploadService _uploadService;

        public MovieService(AppDbContext context, DateUtil dateUtil, IUploadService uploadService)
        {
            _context = context;
            _dateUtil = dateUtil;
            _uploadService = uploadService;
        }

        public async Task<Movie> FindByIdAsync(string id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id && 
                m.Deleted == false) ?? 
                throw new Exception("Movie not found");
            return movie;
        }

        public async Task<IEnumerable<Movie>> FindAllAsync()
        {
            return await _context.Movies
                .Where(m => m.Deleted == false)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<Movie> CreateAsync(CreateMovieRequest req)
        {
            var releaseDate = _dateUtil.GetStringToDate(req.ReleaseDate);
            string imageUrl = "";

            if (req.Poster != null)
                imageUrl = await _uploadService.UploadFileAsync(req.Poster, "Movies");

            var movie = new Movie
            {
                Title = req.Title,
                ImageUrl = imageUrl,
                Duration = req.Duration,
                ReleaseDate = releaseDate,
                IsPublished = _dateUtil.IsDateInRangeOneWeek(releaseDate),
                Description = req.Description
            };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            if (req.ListOfGenres != null)
            {
                foreach (var genreId in req.ListOfGenres)
                {
                    var movieGenres = new MovieGenre
                    {
                        MovieId = movie.Id,
                        GenreId = genreId
                    };

                    await _context.MovieGenres.AddAsync(movieGenres);
                }
            }
            
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<Movie> UpdateAsync(UpdateMovieRequest req, string id)
        {
            if (id == null)
                throw new Exception("Id is required");

            var releaseDate = _dateUtil.GetStringToDate(req.ReleaseDate);
            
            var isMovieExists = await FindByIdAsync(id);

            if (req.Poster != null)
            {
                isMovieExists.ImageUrl = await _uploadService.UploadFileAsync(req.Poster, "Movies");

                if (isMovieExists.ImageUrl != null)
                {
                    var deleteOldImage = await _uploadService.DeleteFileAsync(isMovieExists.ImageUrl);

                    if (!deleteOldImage)
                        throw new Exception("Failed to delete old image");
                }
            }

            isMovieExists.Title = req.Title;
            isMovieExists.Duration = req.Duration;
            isMovieExists.Description = req.Description;
            isMovieExists.ReleaseDate = releaseDate;
            isMovieExists.IsPublished = req.IsPublished;
            isMovieExists.UpdatedAt = DateTime.Now;

            if (req.ListOfGenres != null)
            {
                var movieOldGenres = await _context.MovieGenres
                    .Where(mg => mg.MovieId == id && 
                        mg.Deleted == false)
                    .ToListAsync();
                
                if (movieOldGenres != null)
                {
                    foreach (var movieOldGenre in movieOldGenres)
                    {
                        if (req.ListOfGenres.Contains(movieOldGenre.GenreId))
                        {
                            req.ListOfGenres.Remove(movieOldGenre.GenreId);
                            continue;
                        }

                        movieOldGenre.Deleted = true;
                        movieOldGenre.UpdatedAt = DateTime.Now;
                        _context.MovieGenres.Update(movieOldGenre);
                    }
                }

                foreach (var genreId in req.ListOfGenres)
                {
                    var movieGenres = new MovieGenre
                    {
                        MovieId = isMovieExists.Id,
                        GenreId = genreId
                    };

                    await _context.MovieGenres.AddAsync(movieGenres);
                }
            }
            _context.Movies.Update(isMovieExists);
            await _context.SaveChangesAsync();
            return isMovieExists;
        }

        public async Task<Movie> DeleteAsync(string id)
        {
            if (id == null)
                throw new Exception("Id is required");

            var movie = await FindByIdAsync(id);
            
            movie.Deleted = true;
            movie.UpdatedAt = DateTime.Now;
            
            var movieGenres = await _context.MovieGenres
                .Where(mg => mg.MovieId == id && 
                    mg.Deleted == false)
                .ToListAsync();
            
            if (movieGenres != null)
            {
                foreach (var movieGenre in movieGenres)
                {
                    movieGenre.Deleted = true;
                    movieGenre.UpdatedAt = DateTime.Now;
                    _context.MovieGenres.Update(movieGenre);
                }
            }
            
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();

            return movie;
        }
    }
}