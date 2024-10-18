using MovieApi.Constants;
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
                        Deleted = 0,
                        IsPublished = 1,
                        CreatedBy = "admin"
                    },
                    new Movie
                    {
                        Title = "The Matrix",
                        Duration = 150,
                        Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                        Deleted = 0,
                        IsPublished = 1,
                        CreatedBy = "admin"
                    },
                    new Movie
                    {
                        Title = "The Lord of the Rings: The Return of the King",
                        Duration = 200,
                        Description = "Gandalf and Aragorn lead the World of Men against Sauron's army to draw his gaze from Frodo and Sam as they approach Mount Doom with the One Ring.",
                        Deleted = 0,
                        IsPublished = 1,
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
                    }                    
                };

                await context.Genres.AddRangeAsync(genres);
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

            // Seed users with hashing password
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
        }
    }
}