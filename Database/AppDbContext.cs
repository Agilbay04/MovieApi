using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MovieApi.Entities;

namespace MovieApi.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set unique fields
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(u => u.Code).IsUnique();

            ToSnakeCase(modelBuilder);
        }

        private static void ToSnakeCase(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(Regex.Replace(entity.GetTableName(), "([a-z])([A-Z])", "$1_$2").ToLower());

                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(Regex.Replace(property.GetColumnName(), "([a-z])([A-Z])", "$1_$2").ToLower());
                }
            }
        }
    }
}