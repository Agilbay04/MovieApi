using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MovieApi.Entities;

namespace MovieApi.Database
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; } 
        public DbSet<Studio> Studios { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Ticket> Tickets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set unique fields
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(u => u.Code).IsUnique();
            modelBuilder.Entity<Studio>().HasIndex(u => u.Code).IsUnique();
            modelBuilder.Entity<Price>().HasIndex(u => u.Code).IsUnique();

            SetIdValueToUUID(modelBuilder);
            ToSnakeCase(modelBuilder);
        }

        private static void ToSnakeCase(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entity.GetTableName();
                if (tableName != null)
                {
                    entity.SetTableName(MyRegex().Replace(tableName, "$1_$2").ToLower());
                }

                foreach (var property in entity.GetProperties())
                {
                    var columnName = property.GetColumnName();
                    if (columnName != null)
                    {
                        property.SetColumnName(MyRegex().Replace(columnName, "$1_$2").ToLower());
                    }
                }
            }
        }

        public override int SaveChanges()
        {   
            SetDeletedFalseOnCreate();
            SetCreatedAtOrUpdatedAt();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetDeletedFalseOnCreate();
            SetCreatedAtOrUpdatedAt();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetCreatedAtOrUpdatedAt()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified) &&
                            (e.Entity.GetType().GetProperty("CreatedAt") != null ||
                            e.Entity.GetType().GetProperty("UpdatedAt") != null));

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added && entry.Property("CreatedAt") != null)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified && entry.Property("UpdatedAt") != null)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
                }
            }
        }

        private void SetDeletedFalseOnCreate()
        {
            var newEntities = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added);

            foreach (var entry in newEntities)
            {
                var entity = entry.Entity;
                var deletedProperty = entity.GetType().GetProperty("Deleted");
                if (deletedProperty != null && deletedProperty.CanWrite)
                {
                    deletedProperty.SetValue(entity, false);
                }
            }
        }

        private static void SetIdValueToUUID(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entity.Name).Property("Id").HasDefaultValueSql("NEWID()");
            }
        }

        [GeneratedRegex("([a-z])([A-Z])")]
        private static partial Regex MyRegex();
    }
}