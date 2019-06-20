using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserUserRolesController : ControllerBase
    {
        private IUserUserRolesService userUserRoleService;
        public UserUserRolesController(IUserUserRolesService userUserRoleService)
        {
            this.userUserRoleService = userUserRoleService;
        }

        /// <summary>
        /// Find an userUserRole by the given id.
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /userUserRoles
        ///     [
        ///     {  
        ///        id: 9,
        ///        userId = 7,
        ///        UserRoleId = 5,
        ///        UserRole = "Regular",
        ///        StartTime = 2019-03-09,
        ///        EndTime = null
        ///     }
        ///     ]
        /// </remarks>
        /// <param name="id">The id given as parameter</param>
        /// <returns>A list of userUserRole with the given id</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/UserUserRoles/5
        [HttpGet("{id}", Name = "GetUserUserRole")]

        public IActionResult Get(int id)
        {
            var found = userUserRoleService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }


        /// <summary>
        /// Add an new UserUserRole
        /// </summary>
        ///   /// <remarks>
        /// Sample response:
        ///
        ///     Put /userUserRoles
        ///     {
        ///        userId = 9,
        ///        userRoleName = "Admin"        
        ///     }
        /// </remarks>
        /// <param name="userUserRolePostModel">Input an userUserRole to be added</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]

        public void Post([FromBody] UserUserRolePostModel userUserRolePostModel)
        {
            userUserRoleService.Create(userUserRolePostModel);
        }

    }
}