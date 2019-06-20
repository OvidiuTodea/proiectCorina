using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Validators
{
    public interface ICreateValidator
    {
        ErrorsCollection Validate(UserPostModel userPostModel, MoviesDbContext context);
    }

    public class CreateValidator : ICreateValidator
    {
        public ErrorsCollection Validate(UserPostModel userPostModel, MoviesDbContext context)
        {
            ErrorsCollection errorsCollection = new ErrorsCollection { Entity = nameof(UserPostModel) };
            User existing = context.Users.FirstOrDefault(u => u.Username == userPostModel.UserName);
            if (existing != null)
            {
                errorsCollection.ErrorMessages.Add($"The username {userPostModel.UserName} is already taken !");
            }
            if (userPostModel.Password.Length < 6)
            {
                errorsCollection.ErrorMessages.Add("The password cannot be shorter than 6 characters !");
            }
            if (errorsCollection.ErrorMessages.Count > 0)
            {
                return errorsCollection;
            }
            return null;
        }
    }
}
