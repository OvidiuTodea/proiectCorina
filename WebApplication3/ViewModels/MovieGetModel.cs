using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class MovieGetModel
    {

        public string Title { get; set; }
        public int Duration { get; set; }
        public int Rating { get; set; }
        public DateTime DateAdded { get; set; }
        public int ReleaseYear { get; set; }
        public int NumberOfComments { get; set; }
        [EnumDataType(typeof(Genre))]
        public Genre Genre { get; set; }
        [EnumDataType(typeof(WatchedState))]
        public WatchedState WatchedState { get; set; }
        public DateTime? DateClosed { get; set; }



        public static MovieGetModel FromMovie(Movie movie)
        {
            return new MovieGetModel
            {
                Title = movie.Title,
                Duration = movie.Duration,
                Rating = movie.Rating,
                DateAdded=movie.DateAdded,
                ReleaseYear=movie.ReleaseYear,
                NumberOfComments = movie.Comments.Count,
                Genre = movie.Genre,
                WatchedState = movie.WatchedState,
                DateClosed = movie.DateClosed
            };
        }
    }
}
