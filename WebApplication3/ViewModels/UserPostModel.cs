using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        public DateTime RegistrationDate { get; set; }


        public static User ToUser(UserPostModel userPostModel)
        {
            //UserRole rol = WebApplication3.Models.UserRole.Regular;

            //if (userPostModel.UserRole == "UserManager")
            //{
            //    rol = WebApplication3.Models.UserRole.UserManager;
            //}
            //else if (userPostModel.UserRole == "Admin")
            //{
            //    rol = WebApplication3.Models.UserRole.Admin;
            //}

            return new User
            {
                FirstName = userPostModel.FirstName,
                LastName = userPostModel.LastName,
                Username = userPostModel.UserName,
                Email = userPostModel.Email,
                Password = ComputeSha256Hash(userPostModel.Password),
                RegistrationDate = DateTime.Now

            };
        }

        private static string ComputeSha256Hash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}

