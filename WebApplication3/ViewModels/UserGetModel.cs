using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class UserGetModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserRole UserRole { get; set; }
        public string Token { get; set; }


        public static UserGetModel FromUser(User user)
        {
            return new UserGetModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Username,
                Email = user.Email,
                UserRole = user.UserRole,
                Token = null
            };
        }
    }
}
