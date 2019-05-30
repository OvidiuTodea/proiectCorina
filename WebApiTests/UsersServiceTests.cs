using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using WebApplication3.Models;
using WebApplication3.Services;


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
                Assert.AreEqual(added.Username, result.Username);
            }
        }
    }
}