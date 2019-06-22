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
        public void GetAllShouldReturnCorrectNumberOfPagesForComments()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnCorrectNumberOfPagesForComments))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var movieService = new MovieService(context);
                var commentService = new CommentService(context);
                var added = new MoviePostModel()

                {
                    Title = "Tianic",
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
                            Owner = null
                        }
                    },

                };

                var current = movieService.Create(added, null);
                var allComments = commentService.GetAll(string.Empty, 1);
                //var allComments = commentService.GetAll(string.Empty, 1);
                
                Assert.AreEqual(1, allComments.NumberOfPages);

            }
        }

        [Test]
        public void CreateShouldAddAndReturnTheCreatedComment()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddAndReturnTheCreatedComment))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var commentsService = new CommentService(context);
                var toAdd = new CommentPostModel()

                {

                    Important = true,
                    Text = "A nice movie",

                };

                var added = commentsService.Create(toAdd, null);


                Assert.IsNotNull(added);
                Assert.AreEqual("A nice movie", added.Text);
                Assert.True(added.Important);
            }
        }

        [Test]
        public void UpsertShouldModifyTheGivenComment()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(UpsertShouldModifyTheGivenComment))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var commentsService = new CommentService(context);
                var toAdd = new CommentPostModel()

                {

                    Important = true,
                    Text = "A nice movie",

                };

                var added = commentsService.Create(toAdd, null);
                var update = new CommentPostModel()
                {
                    Important = false
                };

                var toUp = commentsService.Create(update, null);
                var updateResult = commentsService.Upsert(added.Id, added);
                Assert.IsNotNull(updateResult);
                Assert.False(toUp.Important);

            }
        }

        [Test]
        public void DeleteShouldDeleteAGivenComment()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(DeleteShouldDeleteAGivenComment))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var commentsService = new CommentService(context);
                var toAdd = new CommentPostModel()

                {

                    Important = true,
                    Text = "A nice movie",

                };


                var actual = commentsService.Create(toAdd, null);
                var afterDelete = commentsService.Delete(actual.Id);
                int numberOfCommentsInDb = context.Comments.CountAsync().Result;
                var resultComment = context.Comments.Find(actual.Id);


                Assert.IsNotNull(afterDelete);
                Assert.IsNull(resultComment);
                Assert.AreEqual(0, numberOfCommentsInDb);
            }
        }

        [Test]
        public void GetByIdShouldReturnCommentWithCorrectId()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnCommentWithCorrectId))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var commentsService = new CommentService(context);
                var toAdd = new CommentPostModel()

                {

                    Important = true,
                    Text = "A nice movie",

                };


                var current = commentsService.Create(toAdd, null);
                var expected = commentsService.GetById(current.Id);



                Assert.IsNotNull(expected);
                Assert.AreEqual(expected.Text, current.Text);
                Assert.AreEqual(expected.Id, current.Id);
            }
        }

    }
}