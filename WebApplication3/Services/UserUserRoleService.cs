using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.Validators;
using WebApplication3.ViewModels;

namespace WebApplication3.Services
{
    public interface IUserUserRolesService
    {

        IQueryable<UserUserRoleGetModel> GetById(int id);
        string GetUserRoleNameById(int id);
        ErrorsCollection Create(UserUserRolePostModel userUserRolePostModel);

    }

    public class UserUserRolesService : IUserUserRolesService
    {
        private MoviesDbContext context;
        private IUserRoleValidator userRoleValidator;


        public UserUserRolesService(IUserRoleValidator userRoleValidator, MoviesDbContext context)
        {
            this.context = context;
            this.userRoleValidator = userRoleValidator;
        }

        public ErrorsCollection Create(UserUserRolePostModel userUserRolePostModel)
        {
            var errors = userRoleValidator.Validate(userUserRolePostModel, context);
            if (errors != null)
            {
                return errors;
            }

            User user = context
                .Users
                .FirstOrDefault(u => u.Id == userUserRolePostModel.UserId);

            if (user != null)
            {
                UserRole userRole = context
                               .UserRoles
                               .Include(urole => urole.UserUserRoles)
                               .FirstOrDefault(urole => urole.Name == userUserRolePostModel.UserRoleName);

                UserUserRole currentUserUserRole = context
                    .UserUserRoles
                    .Include(uurole => uurole.UserRole)
                    .FirstOrDefault(uurole => uurole.UserId == user.Id && uurole.EndTime == null);

                if (currentUserUserRole == null)
                {
                    context.UserUserRoles.Add(new UserUserRole
                    {
                        User = user,
                        UserRole = userRole,
                        StartTime = DateTime.Now,
                        EndTime = null
                    });

                    context.SaveChanges();
                    return null;
                }


                if (!currentUserUserRole.UserRole.Name.Contains(userUserRolePostModel.UserRoleName))
                {
                    currentUserUserRole.EndTime = DateTime.Now;

                    context.UserUserRoles.Add(new UserUserRole
                    {
                        User = user,
                        UserRole = userRole,
                        StartTime = DateTime.Now,
                        EndTime = null
                    });

                    context.SaveChanges();
                    return null;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public IQueryable<UserUserRoleGetModel> GetById(int id)
        {
            IQueryable<UserUserRole> userUserRole = context
                .UserUserRoles
                .Include(u => u.UserRole)
                .AsNoTracking()
                .Where(uurole => uurole.UserId == id)
                .OrderBy(uurole => uurole.StartTime);

            return userUserRole.Select(uurole => UserUserRoleGetModel.FromUserUserRole(uurole));
        }

        public string GetUserRoleNameById(int id)
        {
            int userRoleId = context
                .UserUserRoles
                .AsNoTracking()
                .FirstOrDefault(uurole => uurole.UserId == id && uurole.EndTime == null)
                .UserRoleId;

            string roleName = context.UserRoles
                  .AsNoTracking()
                  .FirstOrDefault(urole => urole.Id == userRoleId)
                  .Name;

            return roleName;
        }
    }
}
