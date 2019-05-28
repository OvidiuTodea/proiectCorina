using System.Linq;

namespace WebApplication3.Models
{
    public class MoviesDbSeeder
    {
        public static void Initialize(MoviesDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any movies.
            if (context.Movies.Any())
            {
                return;   // DB has been seeded
            }

            context.Movies.AddRange(
                new Movie
                {
                    Title = "SomeMovie",
                    Description = "Description of the movie",
                    Duration = 120,
                    Genre = Genre.Action,
                    ReleaseYear=2019,
                    Director="Corina",
                    Rating=7,
                    Watched=true,
                  

                }
            );
            context.SaveChanges();
        }
    }
}
