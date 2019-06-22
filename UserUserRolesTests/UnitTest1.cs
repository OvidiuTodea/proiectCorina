using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WebApplication3.Models;
using WebApplication3.Services;

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
        public void GetByIdShouldReturnUserRole()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnUserRole))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userUserRolesService = new UserUserRolesService(null, context);

                User userToAdd = new User
                {
                    Email = "anca@yahoo.com",
                    LastName = "Anca",
                    FirstName = "Anca",
                    Password = "123456",
                    RegistrationDate = DateTime.Now,
                    UserUserRoles = new List<UserUserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Newcomer",
                    Description = "..."
                };
                context.UserRoles.Add(addUserRole);
                context.SaveChanges();

                context.UserUserRoles.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

                var userUserRoleGet = userUserRolesService.GetById(1);
                Assert.IsNotNull(userUserRoleGet.FirstOrDefaultAsync(uurole => uurole.EndTime == null));

            }
        }

        [Test]
        public void CreateShouldAddAnUserUserRole()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(CreateShouldAddAnUserUserRole))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userUserRolesService = new UserUserRolesService(null, context);

                User userToAdd = new User
                {
                    Email = "anca@yahoo.com",
                    LastName = "Anca",
                    FirstName = "Anca",
                    Password = "123456",
                    RegistrationDate = DateTime.Now,
                    UserUserRoles = new List<UserUserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Newcomer",
                    Description = "..."
                };
                context.UserRoles.Add(addUserRole);
                context.SaveChanges();

                context.UserUserRoles.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

               
                Assert.NotNull(userToAdd);
                Assert.AreEqual("Newcomer", addUserRole.Name);
                Assert.AreEqual("Anca", userToAdd.FirstName);
            }
        }


        [Test]
        public void GetUserRoleNameByIdShouldReturnUserRoleName()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetUserRoleNameByIdShouldReturnUserRoleName))
              .Options;

            using (var context = new MoviesDbContext(options))
            {
                var userUserRolesService = new UserUserRolesService(null, context);

                User userToAdd = new User
                {
                    Email = "anca@yahoo.com",
                    LastName = "Anca",
                    FirstName = "Anca",
                    Password = "123456",
                    RegistrationDate = DateTime.Now,
                    UserUserRoles = new List<UserUserRole>()
                };
                context.Users.Add(userToAdd);

                UserRole addUserRole = new UserRole
                {
                    Name = "Newcomer",
                    Description = "..."
                };
                context.UserRoles.Add(addUserRole);
                context.SaveChanges();

                context.UserUserRoles.Add(new UserUserRole
                {
                    User = userToAdd,
                    UserRole = addUserRole,
                    StartTime = DateTime.Now,
                    EndTime = null
                });
                context.SaveChanges();

                string userRoleName = userUserRolesService.GetUserRoleNameById(userToAdd.Id);
                Assert.AreEqual("Newcomer", userRoleName);
                Assert.AreEqual("Anca", userToAdd.FirstName);

            }
        }

    }
}