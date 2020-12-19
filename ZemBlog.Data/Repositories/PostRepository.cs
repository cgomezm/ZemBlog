using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zemblog.Domain;
using ZemBlog.Data.Interfaces;

namespace ZemBlog.Data.Services
{
    public class PostRepository : IPostRepository
    {
        private ZemBlogDbContext zemBlogDbContext;

        public PostRepository(ZemBlogDbContext zemBlogDbContext)
        {
            this.zemBlogDbContext = zemBlogDbContext;
        }

        public async Task<int> AddComment(int postId, Comment comment)
        {
            var post = await zemBlogDbContext.Posts.Include(c => c.Comments).Where(p => p.Id == postId).FirstOrDefaultAsync();

            post.Comments.Add(comment);

            return await zemBlogDbContext.SaveChangesAsync();
        }

        public async Task<int> CreatePost(Post post)
        {
            zemBlogDbContext.Posts.Add(post);
            return await zemBlogDbContext.SaveChangesAsync();
        }

        public async Task<int> DeletePost(int postId)
        {
            var post = await zemBlogDbContext.Posts.FindAsync(postId);
            zemBlogDbContext.Posts.Remove(post);
            return await zemBlogDbContext.SaveChangesAsync();
        }

        public async Task<Post> GetPost(int postId)
        {
            return await zemBlogDbContext.Posts.FindAsync(postId);
        }

        public async Task<List<Post>> GetPosts(string author = null)
        {
            return await (from post in zemBlogDbContext.Posts
                            where string.IsNullOrEmpty(author) || post.Author == author
                            select post).AsNoTracking().ToListAsync();
        }

        public async Task<List<Post>> GetPostsByStatus(PostStatus status)
        {
            return await zemBlogDbContext.Posts.Where(p => p.PostStatus == status)
                .Include(p => p.Comments)
                .AsNoTracking().ToListAsync();
        }

        public async Task<int> UpdatePost(int postId, Post post)
        {
            var currentPost = await zemBlogDbContext.Posts.FindAsync(postId);
            currentPost.Text = post.Text;

            return await zemBlogDbContext.SaveChangesAsync();
        }

        public async Task<int> UpdatePostStatus(int postId, PostStatus status)
        {
            var post = await zemBlogDbContext.Posts.FindAsync(postId);
            post.PostStatus = status;

            if (status == PostStatus.Approved)
                post.PublishedDate = DateTime.Now;

            return await zemBlogDbContext.SaveChangesAsync();
        }
    }
}
