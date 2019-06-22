using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication3.Models;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentService commentService;
        private IUsersService usersService;

        public CommentsController(ICommentService commentService, IUsersService usersService)
        {
            this.commentService = commentService;
            this.usersService = usersService;
        }
        /// <summary>
        /// Gets all comments (filtered by a string)
        /// </summary>
        /// <param name="filter">the filter to be applied to comments search</param>
        /// <returns>A list of filtered comments</returns>
        // GET: api/Comments
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public PaginatedList<CommentGetModel> GetAll([FromQuery]string filter, [FromQuery]int page = 1)
        {
            return commentService.GetAll(filter, page);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Regular")]
        public IActionResult Get(int id)
        {
            var found = commentService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Regular")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public void Post([FromBody] CommentPostModel comment)
        {
            User addedBy = usersService.GetCurrentUser(HttpContext);
            commentService.Create(comment, addedBy);
        }

        [Authorize(Roles = "Admin,Regular")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Comment comment)
        {
            var result = commentService.Upsert(id, comment);
            return Ok(result);
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Regular")]
        public IActionResult Delete(int id)
        {
            var result = commentService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
