using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    // https://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api
    //[Authorize]
    //[Route("api/[controller]/[action]")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService _userService;

        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]  //poate fi accesata si daca nu esti logat
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginPostModel login)
        {
            var user = _userService.Authenticate(login.Username, login.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterPostModel registerModel)
        {
            var user = _userService.Register(registerModel);
            if (user == null)
            {
                return BadRequest(new { ErrorMessage = "Username already exists." });
            }
            return Ok(user);
        }

        //[AllowAnonymous]
        [HttpGet]
        [Authorize(Roles = "UserManager,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        //////////////////////////////////////////////////////////////////////////
        ///////////////////////////////  USER CRUD    ////////////////////////////
        //////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// /users
        /// Add a new User:
        /// {
        ///         firstName = "Ovidiu",
        ///         lastName = "Todea",
        ///         userName = "OvidiuTodea",
        ///         email = "ovi@yahoo.com",
        ///         password ="123456",
        ///         userRole = "regular"
        /// }
        /// 
        /// </summary>
        /// <param name="userPostModel"> User which should be added</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles ="UserManager,Admin")]
        [HttpPost]
        public void Post([FromBody] UserPostModel userPostModel)
        {
            _userService.Create(userPostModel);
        }


        /// <summary>
        /// /users/id
        /// Upsert an User
        ///{
        ///         firstName = "Ovidiu",
        ///         lastName = "Todea",
        ///         userName = "OvidiuTodea",
        ///         email = "ovi@yahoo.com",
        ///         password ="123456",
        ///         userRole = "regular"
        /// }
        /// </summary>
        /// <param name="id"> user id</param>
        /// <param name="userPostModel"> user </param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "UserManager,Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserPostModel userPostModel)
        {
            User curentUserLoggedIn = _userService.GetCurrentUser(HttpContext);

            if(curentUserLoggedIn.UserRole == UserRole.UserManager)
            {
                UserGetModel userToUpdate = _userService.GetById(id);

                var dateOfUserRegistration = curentUserLoggedIn.DataRegistered;
                var curentDate = DateTime.Now;
                // var diffMonths = curentDate.Subtract(dateOfUserRegistration).Days / (365.25 / 12); //diferenta de luni

                var diffMonths = curentDate.Month - dateOfUserRegistration.Month;

                if (diffMonths >= 6)
                {
                    var result3 = _userService.Upsert(id, userPostModel);
                    return Ok(result3);
                }

                UserPostModel newUserPost = new UserPostModel
                {
                    FirstName = userPostModel.FirstName,
                    LastName = userPostModel.LastName,
                    UserName = userPostModel.UserName,
                    Email = userPostModel.Email,
                    Password = userPostModel.Password,
                    UserRole = userToUpdate.UserRole.ToString()
                };

                var result2 = _userService.Upsert(id, newUserPost);
                return Ok(result2);

            }

            var result = _userService.Upsert(id, userPostModel);
            return Ok(result);
        }

        /// <summary>
        /// Delete the User with the given id
        /// </summary>
        /// <param name="id">the given id</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "UserManager,Admin")]
        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        { 

            User curentUserLoggedIn = _userService.GetCurrentUser(HttpContext);

            
            if (curentUserLoggedIn.UserRole == UserRole.UserManager)
            {
                UserGetModel userToDelete = _userService.GetById(id);

                if (userToDelete.UserRole.Equals(UserRole.Admin))
                {
                    return NotFound("You are not authorized to delete an Admin User");
                }
                
            }

            var result = _userService.Delete(id);
            if (result == null)
            {
                return NotFound("User with the given id is not found ");
            }
            return Ok(result);
        }

    }
}