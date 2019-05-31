using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{

    public enum Genre
    {
        Action,
        Comedy,
        Horror,
        Thriller
    }
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        [EnumDataType(typeof(Genre))]
        public Genre Genre { get; set; }
        public int ReleaseYear { get; set; }
        public DateTime DateAdded { get; set; }
        public string Director { get; set; }
        [Range(1, 10, ErrorMessage = "Can only be between 1 and 10")]
        public int Rating { get; set; }
        public bool Watched { get; set; }
        public List<Comment> Comments { get; set; }
        public User Owner { get; set; }
    }
}
