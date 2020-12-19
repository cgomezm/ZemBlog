using System;
using System.Collections.Generic;

namespace Zemblog.Domain
{
    public class Post
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime? PublishedDate { get; set; }
        public PostStatus? PostStatus { get; set; }
        public virtual List<Comment> Comments { get; set; }
    }
}
