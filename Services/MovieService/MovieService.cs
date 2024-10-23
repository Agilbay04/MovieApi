using log4net;
using Microsoft.EntityFrameworkCore;
using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Exceptions;
using MovieApi.Requests;
using MovieApi.Requests.Movie;
using MovieApi.Responses.Genre;
using MovieApi.Responses.Movie;
using MovieApi.Services.UploadService;
using MovieApi.Services.UserService;
using MovieApi.Utilities;

namespace MovieApi.Services.MovieService
{
    public class MovieService : IMovieService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(MovieService));

        private readonly AppDbContext _context;

        private readonly DateUtil _dateUtil;

        private readonly IUploadService _uploadService;

        private readonly IUserService _userService;

        public MovieService(
            AppDbContext context, 
            DateUtil dateUtil, 
            IUploadService uploadService, 
            IUserService userService)
        {
            _context = context;
            _dateUtil = dateUtil;
            _uploadService = uploadService;
            _userService = userService;
        }

        public async Task<(Movie, List<Genre>)> FindByIdAsync(string id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id && 
                m.Deleted == false) ?? 
                throw new NotFoundException("Movie not found");

            var listGenres = await _context.MovieGenres
                .Join(_context.Genres, 
                    mg => mg.GenreId, 
                    g => g.Id,
                    (mg, g) => new { mg, g})
                .Where(mg => mg.mg.MovieId == id && 
                    mg.mg.Deleted == false)
                .Select(mg => mg.g)
                .ToListAsync();

            return (movie, listGenres);
        }

        public async Task<List<MovieResponse>> FindAllAsync()
        {
            var movie = await _context.Movies
                .GroupJoin(_context.MovieGenres,
                    m => m.Id,
                    mg => mg.MovieId,
                    (m, mg) => new { m, mg })
                .Where(m => m.m.Deleted == false)
                .Select(m => new MovieResponse
                {
                    Id = m.m.Id,
                    Title = m.m.Title,
                    ImageUrl = m.m.ImageUrl,
                    Duration = m.m.Duration,
                    Description = m.m.Description,
                    IsPublished = m.m.IsPublished ? 
                        "Published" : "Unpublished",
                    ReleaseDate = _dateUtil.GetDateToString(m.m.ReleaseDate),
                    CreatedAt = _dateUtil.GetDateTimeToString(m.m.CreatedAt),
                    UpdatedAt = _dateUtil.GetDateTimeToString(m.m.UpdatedAt),
                    ListOfGenres = m.mg.Select(mg => mg.Genre.Name).ToList()
                })
                .ToListAsync();

            return movie;
        }

        public async Task<(Movie, List<Genre>)> CreateAsync(CreateMovieRequest req)
        {
            using var transction = await _context.Database.BeginTransactionAsync();
            var releaseDate = _dateUtil.GetStringToDate(req.ReleaseDate);
            string imageUrl = "";

            try
            {
                if (req.Poster != null)
                imageUrl = await _uploadService.UploadFileAsync(req.Poster, "Movies");

                var movie = new Movie
                {
                    Title = req.Title,
                    ImageUrl = imageUrl,
                    Duration = req.Duration,
                    ReleaseDate = releaseDate,
                    IsPublished = _dateUtil.IsDateInRangeOneWeek(releaseDate),
                    Description = req.Description,
                    CreatedBy = _userService.GetUserId(),
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
                
                _log.Info($"Admin {_userService.GetUsername()} add new movie data: {movie.Title}");
                await _context.SaveChangesAsync();

                await transction.CommitAsync();

                var listGenres = await _context.MovieGenres
                    .Join(_context.Genres, 
                        mg => mg.GenreId, 
                        g => g.Id,
                        (mg, g) => new { mg, g})
                    .Where(mg => mg.mg.MovieId == movie.Id && 
                        mg.mg.Deleted == false)
                    .Select(mg => mg.g)
                    .ToListAsync();

                return (movie, listGenres);
            }
            catch (Exception ex)
            {
                await transction.RollbackAsync();
                _log.Error("Failed to create new movie", ex);
                throw new BadRequestException("Failed to create new movie");
            }
            
        }

        public async Task<(Movie, List<Genre>)> UpdateAsync(UpdateMovieRequest req, string id)
        {
            using var transction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (id == null)
                throw new BadRequestException("Id is required");

                var releaseDate = _dateUtil.GetStringToDate(req.ReleaseDate);
                
                var (isMovieExists, _) = await FindByIdAsync(id);

                if (req.Poster != null)
                {
                    isMovieExists.ImageUrl = await _uploadService.UploadFileAsync(req.Poster, "Movies");

                    if (isMovieExists.ImageUrl != null)
                    {
                        var deleteOldImage = await _uploadService.DeleteFileAsync(isMovieExists.ImageUrl);

                        if (!deleteOldImage)
                            throw new BadRequestException("Failed to delete old image");
                    }
                }

                isMovieExists.Title = req.Title;
                isMovieExists.Duration = req.Duration;
                isMovieExists.Description = req.Description;
                isMovieExists.ReleaseDate = releaseDate;
                isMovieExists.IsPublished = req.IsPublished;
                isMovieExists.UpdatedAt = DateTime.Now;
                isMovieExists.UpdatedBy = _userService.GetUserId();

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

                await transction.CommitAsync();

                var listGenres = await _context.MovieGenres
                    .Join(_context.Genres, 
                        mg => mg.GenreId, 
                        g => g.Id,
                        (mg, g) => new { mg, g})
                    .Where(mg => mg.mg.MovieId == isMovieExists.Id && 
                        mg.mg.Deleted == false)
                    .Select(mg => mg.g)
                    .ToListAsync();

                _log.Info($"Admin {_userService.GetUsername()} update movie data: {isMovieExists.Title}");
                return (isMovieExists, listGenres);
            }
            catch(Exception ex)
            {
                await transction.RollbackAsync();
                _log.Error("Failed to update movie", ex);
                throw new BadRequestException("Failed to update movie");
            }
        }

        public async Task<(Movie, List<Genre>)> DeleteAsync(string id)
        {
            using var transction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                if (id == null)
                throw new BadRequestException("Id is required");

                var (movie, _) = await FindByIdAsync(id);
                
                movie.Deleted = true;
                movie.UpdatedAt = DateTime.Now;
                movie.UpdatedBy = _userService.GetUserId();
                
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

                await transction.CommitAsync();

                var listGenres = await _context.MovieGenres
                    .Join(_context.Genres, 
                        mg => mg.GenreId, 
                        g => g.Id,
                        (mg, g) => new { mg, g})
                    .Where(mg => mg.mg.MovieId == movie.Id && 
                        mg.mg.Deleted == false)
                    .Select(mg => mg.g)
                    .ToListAsync();

                _log.Info($"Admin {_userService.GetUsername()} delete movie data: {movie.Title}");
                return (movie, listGenres);
            }
            catch(Exception ex)
            {
                await transction.RollbackAsync();
                _log.Error("Failed to delete movie", ex);
                throw new BadRequestException("Failed to delete movie");
            }
        }
    }
}