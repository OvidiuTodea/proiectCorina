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
    public class UserRolesController : ControllerBase
    {
        private IUserRoleService userRoleService;

        public UserRolesController(IUserRoleService userRoleService)
        {
            this.userRoleService = userRoleService;
        }

        /// <summary>
        /// Get all userRoles from DB
        /// </summary>
        /// <returns>A list with all userRoles</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<UserRoleGetModel> GetAll()
        {
            return userRoleService.GetAll();
        }


        /// <summary>
        /// Find an userRole by the given id.
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /userRoles
        ///     {  
        ///        id: 9,
        ///        name = "Regular",
        ///        description = "Default role for new user"
        ///     }
        /// </remarks>
        /// <param name="id">The id given as parameter</param>
        /// <returns>The userRole with the given id</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/UserRoles/5
        [HttpGet("{id}", Name = "GetUserRole")]
        public IActionResult Get(int id)
        {
            var found = userRoleService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }


        /// <summary>
        /// Add an new UserRole
        /// </summary>
        ///   /// <remarks>
        /// Sample response:
        ///
        ///     Post /userRoles
        ///     {
        ///        name = "Regular",
        ///        description = "Default role for new user"
        ///     }
        /// </remarks>
        /// <param name="userPostModel">The input userRole to be added</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]

        public void Post([FromBody] UserRolePostModel userRolePostModel)
        {
            userRoleService.Create(userRolePostModel);
        }


        /// <summary>
        /// Modify an userRole if exists, or add if not exist
        /// </summary>
        /// <param name="id">userRole id to update</param>
        /// <param name="userRolePostDTO">object userRolePostModel to update</param>
        /// Sample request:
        ///     <remarks>
        ///     Put /userRoles/id
        ///     {
        ///        name = "Regular",
        ///        description = "Default role for new user"
        ///     }
        /// </remarks>
        /// <returns>Status 200 daca a fost modificat</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{id}")]

        public IActionResult Put(int id, [FromBody] UserRolePostModel userRolePostModel)
        {
            var result = userRoleService.Upsert(id, userRolePostModel);
            return Ok(result);
        }


        /// <summary>
        /// Delete an userRole
        /// </summary>
        /// <param name="id">UserRole id to delete</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            var result = userRoleService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }




    }
}