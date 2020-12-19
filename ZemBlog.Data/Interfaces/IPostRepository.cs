using System.Collections.Generic;
using System.Threading.Tasks;
using Zemblog.Domain;

namespace ZemBlog.Data.Interfaces
{
    public interface IPostRepository
    {
        /// <summary>
        /// Get all posts and filter by user if provided
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// 
        Task<List<Post>> GetPosts(string author = null);

        /// <summary>
        /// Get posts by status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<List<Post>> GetPostsByStatus(PostStatus status);
        /// <summary>
        /// Get post by its id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<Post> GetPost(int postId);

        /// <summary>
        /// Create a new post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        Task<int> CreatePost(Post post);

        /// <summary>
        /// update a post status by its id
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<int> UpdatePostStatus(int postId, PostStatus status);

        /// <summary>
        /// Updates a post based on its id
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        Task<int> UpdatePost(int postId, Post post);

        /// <summary>
        /// Add comment to post
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<int> AddComment(int postId, Comment comment);

        /// <summary>
        /// Deletes a post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<int> DeletePost(int postId);
    }
}
