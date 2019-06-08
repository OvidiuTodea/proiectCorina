﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Services
{
    public interface IUsersService
    {
        LoginGetModel Authenticate(string username, string password);
        LoginGetModel Register(RegisterPostModel registerInfo);
        User GetCurrentUser(HttpContext httpContext);
        IEnumerable<LoginGetModel> GetAll();

        UserGetModel GetById(int id);
        UserGetModel Create(UserPostModel userPostModel);
        UserGetModel Upsert(int id, UserPostModel userPostModel);
        UserGetModel Delete(int id);
    }

    public class UsersService : IUsersService
    {
        private MoviesDbContext context;
        private readonly AppSettings appSettings;

        public UsersService(MoviesDbContext context, IOptions<AppSettings> appSettings)
        {
            this.context = context;
            this.appSettings = appSettings.Value;
        }

        public LoginGetModel Authenticate(string username, string password)
        {
            var user = context.Users
                .SingleOrDefault(x => x.Username == username &&
                                 x.Password == ComputeSha256Hash(password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            //token descriptor contine informatia continuta in token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString()),
                    new Claim(ClaimTypes.UserData, user.DataRegistered.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7), //cand expira tokenul
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new LoginGetModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            };
            
            return result;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            // TODO: also use salt
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public LoginGetModel Register(RegisterPostModel registerInfo)
        {
            User existing = context.Users.FirstOrDefault(u => u.Username == registerInfo.Username);
            if (existing != null)
            {
                return null;
            }

            context.Users.Add(new User
            {
                Email = registerInfo.Email,
                LastName = registerInfo.LastName,
                FirstName = registerInfo.FirstName,
                Password = ComputeSha256Hash(registerInfo.Password),
                Username = registerInfo.Username,
                UserRole = UserRole.Regular
            });
            context.SaveChanges();  
            return Authenticate(registerInfo.Username, registerInfo.Password);
        }

        public IEnumerable<LoginGetModel> GetAll()
        {
            // return users without passwords
            return context.Users.Select(user => new LoginGetModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = null
            });
        }

        public User GetCurrentUser(HttpContext httpContext)
        {
            string username = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            //string accountType = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.AuthenticationMethod).Value;
            //return _context.Users.FirstOrDefault(u => u.Username == username && u.AccountType.ToString() == accountType);
            return context.Users.FirstOrDefault(u => u.Username == username);

        }

        ////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////         USER   CRUD          /////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////



        public UserGetModel GetById(int id)
        {
            User user = context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == id);
            return UserGetModel.FromUser(user);
        }

        public UserGetModel Create(UserPostModel userPostModel)
        {
            User toAdd = UserPostModel.ToUser(userPostModel);

            context.Users.Add(toAdd);
            context.SaveChanges();
            return UserGetModel.FromUser(toAdd);
        }

        public UserGetModel Upsert(int id, UserPostModel userPostModel)
        {
            var existing = context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                User toAdd = UserPostModel.ToUser(userPostModel);
                context.Users.Add(toAdd);
                context.SaveChanges();
                return UserGetModel.FromUser(toAdd);
            }

            User toUpdate = UserPostModel.ToUser(userPostModel);
            toUpdate.Id = id;
            context.Users.Update(toUpdate);
            context.SaveChanges();
            return UserGetModel.FromUser(toUpdate);
        }

        public UserGetModel Delete(int id)
        {
            var existing = context.Users.FirstOrDefault(u => u.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Users.Remove(existing);
            context.SaveChanges();

            return UserGetModel.FromUser(existing);
        }


    }
}
