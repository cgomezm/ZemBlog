using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Zemblog.Domain;
using ZemBlog.Data.Interfaces;

namespace Zemblog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PendingPostsController : ControllerBase
    {
        private IPostRepository postRepository;

        public PendingPostsController(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        /// <summary>
        /// Get all posts in pending approval.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await postRepository.GetPostsByStatus(PostStatus.PendingApproval));
        }

        /// <summary>
        /// Update post status
        /// </summary>
        /// <param name="action">1. Approve 2. Reject</param>
        /// <param name="postId">Id of post to update</param>
        /// <returns></returns>
        [HttpPost("updatestatus")]        
        public async Task<IActionResult> Post(int postId, string action)
        {
            return Ok(await postRepository.UpdatePostStatus(postId,(PostStatus)Enum.Parse(typeof(PostStatus), action)));
        }
    }
}
