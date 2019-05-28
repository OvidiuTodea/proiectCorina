using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Models
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Comment>Comments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
