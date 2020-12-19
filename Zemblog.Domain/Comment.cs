namespace Zemblog.Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
