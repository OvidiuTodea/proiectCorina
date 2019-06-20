using System;
using System.Collections.Generic;
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
        [EnumDataType(typeof(WatchedState))]
        public string WatchedState { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateClosed { get; set; }
        public List<Comment> Comments { get; set; }

        public static Movie ToMovie(MoviePostModel movie)
        {
            Genre genre = Models.Genre.Action;
            if (movie.Genre == "Comedy") genre = Models.Genre.Comedy;
            if (movie.Genre == "Horror") genre = Models.Genre.Horror;
            if (movie.Genre == "Thriller") genre = Models.Genre.Thriller;

            WatchedState watchedState = Models.WatchedState.Yes;
            if (movie.WatchedState == "No") watchedState = Models.WatchedState.No;
            if (movie.WatchedState == "InProgress") watchedState = Models.WatchedState.No;


            return new Movie
            {
                Title = movie.Title,
                Duration = movie.Duration,
                Genre = genre,
                WatchedState = watchedState,
                DateAdded = movie.DateAdded,
                DateClosed = movie.DateClosed,
                Comments = movie.Comments
            };
        }
    }
}
