using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ICommentService commentService;
        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }
        /// <summary>
        /// Gets all comments
        /// </summary>
        /// <param name="filter">the filter to be applied to comments search</param>
        /// <returns>A list of filtered comments</returns>
        // GET: api/Comments
        [HttpGet]
        public IEnumerable<CommentGetModel> Get([FromQuery]string filter)
        {
            return commentService.GetAll(filter);
        }
    }
}
