using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class UserPostModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }

        public static User ToUser(UserPostModel userPostModel)
        {
            UserRole rol = WebApplication3.Models.UserRole.Regular;

            if (userPostModel.UserRole == "UserManager")
            {
                rol = WebApplication3.Models.UserRole.UserManager;
            }
            else if (userPostModel.UserRole == "Admin")
            {
                rol = WebApplication3.Models.UserRole.Admin;
            }

            return new User
            {
                FirstName = userPostModel.FirstName,
                LastName = userPostModel.LastName,
                Username = userPostModel.UserName,
                Email = userPostModel.Email,
                Password = userPostModel.Password,
                UserRole = rol
            };
        }
    }
}

