using Microsoft.EntityFrameworkCore;
using Series.Models;

namespace Series.Data
{
    public class SeriesContext : DbContext
    {
        public DbSet<Series.Models.Series> Series { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<SeriesGenre> SeriesGenres { get; set; }
        public DbSet<SeriesCreator> SeriesCreators { get; set; }

        public SeriesContext(DbContextOptions<SeriesContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeriesGenre>().HasKey(sg => new { sg.SeriesId, sg.GenreId });
            modelBuilder.Entity<SeriesCreator>().HasKey(sc => new { sc.SeriesId, sc.CreatorId });
        }
    }
}

