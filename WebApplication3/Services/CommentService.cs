using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Services
{
    public interface ICommentService
    {
        IEnumerable<CommentGetModel> GetAll(string filter);
    }
    public class CommentService : ICommentService
    {
        private MoviesDbContext context;
        public CommentService(MoviesDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<CommentGetModel> GetAll(string filter)
        {

            IQueryable<Movie> result = context.Movies.Include(c => c.Comments);

            List<CommentGetModel> resultComments = new List<CommentGetModel>();
            List<CommentGetModel> resultCommentsNoFilter = new List<CommentGetModel>();

            foreach (Movie m in result)
            {
                m.Comments.ForEach(c =>
                {
                    if (c.Text == null || filter == null)
                    {
                        CommentGetModel comment = new CommentGetModel
                        {
                            Important = c.Important,
                            Text = c.Text,
                            MovieId = m.Id

                        };
                        resultCommentsNoFilter.Add(comment);
                    }
                    else if (c.Text.Contains(filter))
                    {
                        CommentGetModel comment = new CommentGetModel
                        {
                            Important = c.Important,
                            Text = c.Text,
                            MovieId = m.Id

                        };
                        resultComments.Add(comment);
                    }
                });
            }
            if (filter == null)
            {
                return resultCommentsNoFilter;
            }
            return resultComments;
        }
    }
}
