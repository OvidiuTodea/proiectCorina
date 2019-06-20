using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class UserUserRoleGetModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserRoleId { get; set; }
        public string UserRoleName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public static UserUserRoleGetModel FromUserUserRole(UserUserRole userUserRole)
        {

            return new UserUserRoleGetModel
            {

                Id = userUserRole.Id,
                UserId = userUserRole.UserId,
                UserRoleId = userUserRole.UserRoleId,
                UserRoleName = userUserRole.UserRole.Name,
                StartTime = userUserRole.StartTime,
                EndTime = userUserRole.EndTime
            };
        }

    }
}
