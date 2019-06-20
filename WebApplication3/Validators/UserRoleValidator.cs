using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Validators
{
    public interface IUserRoleValidator
    {
        ErrorsCollection Validate(UserUserRolePostModel userUserRolePosModel, MoviesDbContext context);
    }

    public class UserRoleValidator : IUserRoleValidator
    {
        public ErrorsCollection Validate(UserUserRolePostModel userUserRolePosModel, MoviesDbContext context)
        {
            ErrorsCollection errorsCollection = new ErrorsCollection { Entity = nameof(UserUserRolePostModel) };


            List<string> userRoles = context

                .UserRoles
                .Select(userRole => userRole.Name)
                .ToList();


            if (!userRoles.Contains(userUserRolePosModel.UserRoleName))
            {
                errorsCollection.ErrorMessages.Add($"The UserRole {userUserRolePosModel.UserRoleName} does not exist!");
            }

            if (errorsCollection.ErrorMessages.Count > 0)
            {
                return errorsCollection;
            }
            return null;
        }
    }
}
