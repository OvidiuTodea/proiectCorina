namespace WebApplication3.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Important { get; set; }
        public User Owner { get; set; }
        public Movie Movie { get; set; }
    }
}
