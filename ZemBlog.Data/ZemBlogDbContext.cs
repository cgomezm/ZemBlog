using Microsoft.EntityFrameworkCore;
using Zemblog.Domain;

namespace ZemBlog.Data
{
    public class ZemBlogDbContext : DbContext
    {
        public ZemBlogDbContext(DbContextOptions<ZemBlogDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
    }



}
