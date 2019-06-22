using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Services
{
    public interface IUserRoleService
    {
        UserRoleGetModel GetById(int id);
        UserRoleGetModel Delete(int id);
        IEnumerable<UserRoleGetModel> GetAll();
        UserRoleGetModel Create(UserRolePostModel userRolePostModel);
        UserRoleGetModel Upsert(int id, UserRolePostModel userRolePostModel);

    }

    public class UserRoleService : IUserRoleService
    {
        private MoviesDbContext context;

        public UserRoleService(MoviesDbContext context)
        {
            this.context = context;
        }



        public UserRoleGetModel Create(UserRolePostModel userRolePostModel)
        {
            UserRole toAdd = UserRolePostModel.ToUserRole(userRolePostModel);

            context.UserRoles.Add(toAdd);
            context.SaveChanges();
            return UserRoleGetModel.FromUserRole(toAdd);
        }

        public UserRoleGetModel Delete(int id)
        {
            var existing = context
                .UserRoles
                .FirstOrDefault(urole => urole.Id == id);
            if (existing == null)
            {
                return null;
            }

            context.UserRoles.Remove(existing);
            context.SaveChanges();

            return UserRoleGetModel.FromUserRole(existing);
        }

        public IEnumerable<UserRoleGetModel> GetAll()
        {
            return context.UserRoles.Select(userRol => UserRoleGetModel.FromUserRole(userRol));
        }

        public UserRoleGetModel GetById(int id)
        {
            UserRole userRole = context
                .UserRoles
                .AsNoTracking()
                .FirstOrDefault(urole => urole.Id == id);

            return UserRoleGetModel.FromUserRole(userRole);
        }

        public UserRoleGetModel Upsert(int id, UserRolePostModel userRolePostModel)
        {
            var existing = context.UserRoles.AsNoTracking().FirstOrDefault(urole => urole.Id == id);
            if (existing == null)
            {
                UserRole toAdd = UserRolePostModel.ToUserRole(userRolePostModel);
                context.UserRoles.Add(toAdd);
                context.SaveChanges();
                return UserRoleGetModel.FromUserRole(toAdd);
            }

            UserRole Up = UserRolePostModel.ToUserRole(userRolePostModel);
            Up.Id = id;
            context.UserRoles.Update(Up);
            context.SaveChanges();
            return UserRoleGetModel.FromUserRole(Up);
        }
    }
}
