using System.ComponentModel.DataAnnotations;
using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class MoviePostModel
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        [EnumDataType(typeof(Genre))]
        public string Genre { get; set; }

        public static Movie ToMovie(MoviePostModel movie)
        {
            Genre genre = Models.Genre.Action;
            if (movie.Genre == "Comedy") genre = Models.Genre.Comedy;
            if (movie.Genre == "Horror") genre = Models.Genre.Horror;
            if (movie.Genre == "Thriller") genre = Models.Genre.Thriller;


            return new Movie
            {
                Title = movie.Title,
                Duration = movie.Duration,
                Genre = genre
            };
        }
    }
}
