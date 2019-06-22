using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using WebApplication3.Models;
using WebApplication3.Services;
using WebApplication3.Validators;
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
        public void ValidRegisterShouldCreateANewUser()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateANewUser))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, validator, null, null, config);
                var added = new RegisterPostModel()

                {

                    FirstName = "Ovi",
                    LastName = "Todea",
                    Username = "otodea",
                    Email = "o@gmail.com",
                    Password = "123456"

                //public string FirstName { get; set; }
                //public string LastName { get; set; }
                //public string Username { get; set; }
                //// [EmailAddress]
                //public string Email { get; set; }
                //// [StringLength(100, MinimumLength = 6)]
                //public string Password { get; set; }
                };
                var result = usersService.Register(added);

                Assert.IsNull(result);
                
             

            }
        }
    }
}