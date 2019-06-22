using WebApplication3.Models;

namespace WebApplication3.ViewModels
{
    public class CommentGetModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }
        public int MovieId { get; set; }

        public static CommentGetModel FromComment(Comment comment)
        {

            return new CommentGetModel
            {
                Id = comment.Id,
                Text = comment.Text,
                MovieId = comment.Movie.Id,
                Important = comment.Important
            };
        }
    }
}
