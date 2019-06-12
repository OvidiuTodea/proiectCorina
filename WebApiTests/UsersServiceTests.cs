using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using WebApplication3.Models;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace Tests
{
    public class UsersServiceTests
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
        public void ValidRegisterShouldCreateANewUser()
        {
            // se ruleaza in Package Manager Console pt InMEmoryDatabases  :  
            // Install -Package Microsoft.EntityFrameworkCore.InMemory

            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateANewUser))// "ValidRegisterShouldCreateANewUser")
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var usersService = new UsersService(context, config);
                var added = new WebApplication3.ViewModels.RegisterPostModel
                {
                    Email = "ovi@yahoo.com",
                    FirstName = "ovidiu",
                    LastName = "ovidiu",
                    Password = "123456",
                    Username = "test_username"
                };
                var result = usersService.Register(added);

                Assert.IsNotNull(result);
                Assert.AreEqual(added.Username, result.UserName);
            }
        }

        [Test]
        public void InvalidRegisterShouldNotCerateANewUserWithTheSameUsername()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(InvalidRegisterShouldNotCerateANewUserWithTheSameUsername))
                .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userService = new UsersService(context, config);

                var added1 = new WebApplication3.ViewModels.RegisterPostModel
                {
                    Email = "ovi@yahoo.com",
                    FirstName = "ovi1",
                    LastName = "todea1",
                    Password = "123456",
                    Username = "user_ovi"

                };
                var added2 = new WebApplication3.ViewModels.RegisterPostModel
                {
                    Email = "todea@yahoo.com",
                    FirstName = "ovi2",
                    LastName = "todea2",
                    Password = "123456",
                    Username = "user_ovi"

                };

                userService.Register(added1);
                var result = userService.Register(added2);

                Assert.AreEqual(null, result);
            }

        }
        [Test]
        public void ValidAuthentificationShouldAuthenticateValidUser()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(ValidAuthentificationShouldAuthenticateValidUser))
                .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userService = new UsersService(context, config);

                var addedUser = new WebApplication3.ViewModels.RegisterPostModel
                {
                    Email = "ovi@yahoo.com",
                    FirstName = "ovi1",
                    LastName = "todea1",
                    Password = "123456",
                    Username = "user_ovi"

                };

                var addResult = userService.Register(addedUser);

                Assert.IsNotNull(addResult);
                Assert.AreEqual(addedUser.Username, addResult.UserName);

                var authentificate = new WebApplication3.ViewModels.LoginGetModel
                {
                    Email = "ovi@yahoo.com",
                    Username = "user_ovi"
                };

                var result = userService.Authenticate(addedUser.Username, addedUser.Password);

                Assert.IsNotNull(result);

                Assert.AreEqual(authentificate.Username, result.UserName);
            }

        }
        [Test]
        public void InvalidAuthentificationShouldNotAuthenticateUserWithInvalidPassword()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(InvalidAuthentificationShouldNotAuthenticateUserWithInvalidPassword))
                .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userService = new UsersService(context, config);

                var addedUser = new WebApplication3.ViewModels.RegisterPostModel
                {
                    Email = "ovi@yahoo.com",
                    FirstName = "ovi1",
                    LastName = "todea1",
                    Password = "123456",
                    Username = "user_ovi"

                };

                var addResult = userService.Register(addedUser);

                Assert.IsNotNull(addResult);
                Assert.AreEqual(addedUser.Username, addResult.UserName);

                var authentificate = new WebApplication3.ViewModels.LoginGetModel
                {
                    Email = "ovi@yahoo.com",
                    Username = "user_ovi"
                };

                var result = userService.Authenticate(addedUser.Username, "012345");

                Assert.AreEqual(null, result);
            }

        }
        [Test]
        public void ValidGetAllShouldReturnAllUsers()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseInMemoryDatabase(databaseName: nameof(ValidGetAllShouldReturnAllUsers))
                .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userService = new UsersService(context, config);

                var addedUser1 = new WebApplication3.ViewModels.RegisterPostModel
                {
                    Email = "ovi@yahoo.com",
                    FirstName = "ovi1",
                    LastName = "todea1",
                    Password = "123456",
                    Username = "user_ovi_1"

                };
                var addedUser2 = new WebApplication3.ViewModels.RegisterPostModel
                {
                    Email = "ovi@yahoo.com",
                    FirstName = "ovi1",
                    LastName = "todea1",
                    Password = "123456",
                    Username = "user_ovi_2"

                };

                var addedUser3 = new WebApplication3.ViewModels.RegisterPostModel
                {
                    Email = "ovi@yahoo.com",
                    FirstName = "ovi1",
                    LastName = "todea1",
                    Password = "123456",
                    Username = "user_ovi_3"

                };

                UserGetModel user1 = userService.Register(addedUser1);
                UserGetModel user2 = userService.Register(addedUser2);
                UserGetModel user3 = userService.Register(addedUser3);
               

                List<UserGetModel> actual = new List<UserGetModel>();

                user1.Token = null;
                user2.Token = null;
                user3.Token = null;

                actual.Add(user1);
                actual.Add(user2);
                actual.Add(user3);

                IEnumerable<UserGetModel> result = userService.GetAll();
                IEnumerable<UserGetModel> expected = actual.AsEnumerable();

                Assert.IsTrue(expected.SequenceEqual(actual));

            }

        }
    }
}