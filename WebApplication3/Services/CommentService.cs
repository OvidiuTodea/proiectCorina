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
        Comment Create(CommentPostModel comment, User addedBy);
        Comment Upsert(int id, Comment comment);
        Comment Delete(int id);
        Comment GetById(int id);
    }
    public class CommentService : ICommentService
    {
        private MoviesDbContext context;
        public CommentService(MoviesDbContext context)
        {
            this.context = context;
        }

        public Comment Create(CommentPostModel comment, User addedBy)
        {
            Comment createdComment = CommentPostModel.ToComment(comment);
            createdComment.Owner = addedBy;
            context.Comments.Add(createdComment);
            context.SaveChanges();
            return createdComment;
        }

        public Comment Delete(int id)
        {
            var existing = context.Comments.FirstOrDefault(comment => comment.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Comments.Remove(existing);
            context.SaveChanges();
            return existing;
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

        public Comment GetById(int id)
        {
            return context.Comments.FirstOrDefault(c => c.Id == id);
        }

        public Comment Upsert(int id, Comment comment)
        {
            var existing = context.Comments.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (existing == null)
            {
                context.Comments.Add(comment);
                context.SaveChanges();
                return comment;

            }

            comment.Id = id;
            context.Comments.Update(comment);
            context.SaveChanges();
            return comment;
        }
    }
}
