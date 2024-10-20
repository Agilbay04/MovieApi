using MovieApi.Database;
using MovieApi.Entities;
using MovieApi.Utilities;

namespace MovieApi.Seeders
{
    public static class AppDbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Seed movies
            if (!context.Movies.Any())
            {
                var movies = new List<Movie>() 
                {
                    new Movie
                    {
                        Title = "The Godfather",
                        Duration = 175,
                        Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
                        CreatedBy = "admin"
                    },
                    new Movie
                    {
                        Title = "The Matrix",
                        Duration = 150,
                        Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                        CreatedBy = "admin"
                    },
                    new Movie
                    {
                        Title = "The Lord of the Rings: The Return of the King",
                        Duration = 200,
                        Description = "Gandalf and Aragorn lead the World of Men against Sauron's army to draw his gaze from Frodo and Sam as they approach Mount Doom with the One Ring.",
                        CreatedBy = "admin"
                    }
                };

                await context.Movies.AddRangeAsync(movies);
                await context.SaveChangesAsync();
            }            

            // Seed genres
            if (!context.Genres.Any())
            {
                var genres = new List<Genre>()
                {
                    new Genre
                    {
                        Name = "Action",
                        CreatedBy = "admin"
                    },
                    new Genre
                    {
                        Name = "Drama",
                        CreatedBy = "admin"
                    },
                    new Genre
                    {
                        Name = "Comedy",
                        CreatedBy = "admin"
                    },
                    new Genre
                    {
                        Name = "Horror",
                        CreatedBy = "admin"
                    },
                    new Genre
                    {
                        Name = "Thriller",
                        CreatedBy = "admin"
                    },
                    new Genre
                    {
                        Name = "Romance",
                        CreatedBy = "admin"
                    },
                    new Genre
                    {
                        Name = "Sci-Fi",
                        CreatedBy = "admin"
                    }               
                };

                await context.Genres.AddRangeAsync(genres);
                await context.SaveChangesAsync();
            }

            // seed movie genres
            if (!context.MovieGenres.Any())
            {
                var movieGenres = new List<MovieGenre>()
                {
                    new MovieGenre
                    {
                        MovieId = context.Movies.Where(m => m.Title == "The Godfather").Select(m => m.Id).FirstOrDefault(),
                        GenreId = context.Genres.Where(g => g.Name == "Drama").Select(g => g.Id).FirstOrDefault(),
                        CreatedBy = "admin"
                    },
                    new MovieGenre
                    {
                        MovieId = context.Movies.Where(m => m.Title == "The Godfather").Select(m => m.Id).FirstOrDefault(),
                        GenreId = context.Genres.Where(g => g.Name == "Action").Select(g => g.Id).FirstOrDefault(),
                        CreatedBy = "admin"
                    },
                    new MovieGenre
                    {
                        MovieId = context.Movies.Where(m => m.Title == "The Matrix").Select(m => m.Id).FirstOrDefault(),
                        GenreId = context.Genres.Where(g => g.Name == "Action").Select(g => g.Id).FirstOrDefault(),
                        CreatedBy = "admin"
                    },
                    new MovieGenre
                    {
                        MovieId = context.Movies.Where(m => m.Title == "The Matrix").Select(m => m.Id).FirstOrDefault(),
                        GenreId = context.Genres.Where(g => g.Name == "Sci-Fi").Select(g => g.Id).FirstOrDefault(),
                        CreatedBy = "admin"
                    },
                    new MovieGenre
                    {
                        MovieId = context.Movies.Where(m => m.Title == "The Lord of the Rings: The Return of the King").Select(m => m.Id).FirstOrDefault(),
                        GenreId = context.Genres.Where(g => g.Name == "Action").Select(g => g.Id).FirstOrDefault(),
                        CreatedBy = "admin"
                    }
                };

                await context.MovieGenres.AddRangeAsync(movieGenres);
                await context.SaveChangesAsync();
            }

            // Seed roles
            if (!context.Roles.Any())
            {
                var roles = new List<Role>()
                {
                    new Role
                    {
                        Code = "admin",
                        Name = "Admin",
                        CreatedBy = "admin"
                    },
                    new Role
                    {
                        Code = "customer",
                        Name = "Customer",
                        CreatedBy = "admin"
                    }
                };

                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }

            // Seed users
            if (!context.Users.Any())
            {
                var pwdAdmin = PasswordUtil.HashPassword("admin123");
                var pwdCustomer = PasswordUtil.HashPassword("customer123");
                var users = new List<User>()
                {
                    new User
                    {
                        Name = "Admin",
                        Username = "admin",
                        RoleId = context.Roles.Where(r => r.Code == AppConstant.ROLE_ADMIN).Select(r => r.Id).FirstOrDefault(),
                        Salt = pwdAdmin.salt,
                        Password = pwdAdmin.hashedPassword,
                        CreatedBy = "admin",
                    },
                    new User
                    {
                        Name = "Customer",
                        Username = "customer",
                        RoleId = context.Roles.Where(r => r.Code == AppConstant.ROLE_CUSTOMER).Select(r => r.Id).FirstOrDefault(),
                        Salt = pwdCustomer.salt,
                        Password = pwdCustomer.hashedPassword,
                        CreatedBy = "admin",
                    }
                };

                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
            }

            // Seed studios
            if (!context.Studios.Any())
            {
                var studios = new List<Studio>()
                {
                    new Studio
                    {
                        Code = "S01",
                        Name = "Studio 1",
                        Facility = "Dolby Atmos",
                        TotalSeats = 50,
                        CreatedBy = "admin"
                    },
                    new Studio
                    {
                        Code = "S02",
                        Name = "Studio 2",
                        Facility = "Dolby Atmos",
                        TotalSeats = 100,
                        CreatedBy = "admin"
                    },
                    new Studio
                    {
                        Code = "S03",
                        Name = "Studio 3",
                        Facility = "Dolby Atmos",
                        TotalSeats = 100,
                        CreatedBy = "admin"
                    }
                };

                await context.Studios.AddRangeAsync(studios);
                await context.SaveChangesAsync();
            }

            // Seed prices
            if (!context.Prices.Any())
            {
                var prices = new List<Price>()
                {
                    new Price
                    {
                        Code = "weekday",
                        Name = "Weekday",
                        Description = "Price for weekday",
                        PriceValue = 30000,
                        CreatedBy = "admin"
                    },
                    new Price
                    {
                        Code = "weekend",
                        Name = "Weekend",
                        Description = "Price for weekend",
                        PriceValue = 35000,
                        CreatedBy = "admin"
                    },
                    new Price
                    {
                        Code = "primetime",
                        Name = "Primetime",
                        Description = "Price for movie release day",
                        PriceValue = 40000,
                        CreatedBy = "admin"
                    }
                };                

                await context.Prices.AddRangeAsync(prices);
                await context.SaveChangesAsync();
            }

        }
    }
}