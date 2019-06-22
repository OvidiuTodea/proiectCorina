using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WebApplication3.Models;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace Tests
{
    public class Tests
    {
        private IOptions<AppSettings> config;
        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                Secret = "dsadhjcghduihdfhdifd8ih"
            });
        }

        [Test]
        public void GetAllShouldReturnCorrectNumberOfPagesForMovies()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPagesForMovies))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var movieService = new MovieService(context);
                var added = movieService.Create(new WebApplication3.ViewModels.MoviePostModel

                {
                    Title = "Tianic",
                    Duration = 100,
                    Genre = "Thriller",
                    WatchedState = "Yes",
                    DateAdded = DateTime.Parse("2019-06-15T00:00:00"),
                    DateClosed = null,

                    //Title = movie.Title,
                    //Duration = movie.Duration,
                    //Genre = genre,
                    //WatchedState = watchedState,
                    //DateAdded = movie.DateAdded,
                    //DateClosed = movie.DateClosed,
                    //Comments = movie.Comments

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "A nice movie",

                        }
                    },

                }, null);

                DateTime from = DateTime.Parse("2019-06-13T00:00:00");
                DateTime to = DateTime.Parse("2019-06-19T00:00:00");

                var allTasks = movieService.GetAll(1, from, to);
                Assert.AreEqual(1, allTasks.Entries.Count);
                Assert.IsNotNull(allTasks);
            }
        }



        [Test]
        public void CreateShouldAddAndReturnTheCreatedMovie()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddAndReturnTheCreatedMovie))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var movieService = new MovieService(context);
                var added = movieService.Create(new WebApplication3.ViewModels.MoviePostModel

                {
                    Title = "Titanic",
                    Duration = 100,
                    Genre = "Thriller",
                    WatchedState = "Yes",
                    DateAdded = DateTime.Parse("2019-06-15T00:00:00"),
                    DateClosed = null,

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "A nice movie",

                        }
                    },

                }, null);


                Assert.IsNotNull(added);
                Assert.AreEqual("Titanic", added.Title);
                Assert.AreNotEqual("Mask", added.Title);

            }
        }

        [Test]
        public void UpsertShouldModifyTheGivenMovie()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(UpsertShouldModifyTheGivenMovie))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var movieService = new MovieService(context);
                var added = new MoviePostModel()

                {
                    Title = "Titanic",
                    Duration = 100,
                    Genre = "Thriller",
                    WatchedState = "Yes",
                    DateAdded = DateTime.Parse("2019-06-15T00:00:00"),
                    DateClosed = null,

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "A nice movie",

                        }
                    },

                };

                var toAdd = movieService.Create(added, null);
                var update = new MoviePostModel()
                {
                    Title = "Updated"
                };

                var toUp = movieService.Create(update, null);
                var updateResult = movieService.Upsert(toUp.Id, toUp);


                Assert.IsNotNull(updateResult);
                Assert.AreEqual(toUp.Title, updateResult.Title);

            }
        }

        [Test]
        public void DeleteMovieWithCommentsShouldDeleteMoviesAndComments()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteMovieWithCommentsShouldDeleteMoviesAndComments))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var moviesService = new MovieService(context);

                var expected = new MoviePostModel()
                {
                    Title = "Titanic",
                    Duration = 100,
                    Genre = "Thriller",
                    WatchedState = "Yes",
                    DateAdded = DateTime.Parse("2019-06-15T00:00:00"),
                    DateClosed = null,

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "A nice movie",

                        }
                    },

                };

                var actual = moviesService.Create(expected, null);
                var afterDelete = moviesService.Delete(actual.Id);
                int numberOfCommentsInDb = context.Comments.CountAsync().Result;
                var resultExpense = context.Movies.Find(actual.Id);

                Assert.IsNotNull(afterDelete);
                Assert.IsNull(resultExpense);
                Assert.AreEqual(0, numberOfCommentsInDb);
            }
        }

        [Test]
        public void GetByIdShouldReturnTaskWithCorrectId()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnTaskWithCorrectId))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var movieService = new MovieService(context);
                var added = new MoviePostModel()

                {
                    Title = "Titanic",
                    Duration = 100,
                    Genre = "Thriller",
                    WatchedState = "Yes",
                    DateAdded = DateTime.Parse("2019-06-15T00:00:00"),
                    DateClosed = null,

                    Comments = new List<Comment>()
                    {
                        new Comment
                        {
                            Important = true,
                            Text = "A nice movie",

                        }
                    },

                };

                var current = movieService.Create(added, null);
                var expected = movieService.GetById(current.Id);

                Assert.IsNotNull(expected);
                Assert.AreEqual(expected.Title, current.Title);
                Assert.AreEqual(expected.Duration, current.Duration);
                Assert.AreEqual(expected.WatchedState, current.WatchedState);
                Assert.AreEqual(expected.Genre, current.Genre);
                Assert.AreEqual(expected.Id, current.Id);

            }
        }
    }
}