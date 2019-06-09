using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Models
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity => {
                entity.HasIndex(u => u.Username).IsUnique();
            });

            builder.Entity<Comment>()
               .HasOne(e => e.Movie)
               .WithMany(c => c.Comments)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Movie>()
              .HasOne(t => t.Owner)
              .WithMany(c => c.Movies)
              .OnDelete(DeleteBehavior.Cascade);

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment>Comments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
