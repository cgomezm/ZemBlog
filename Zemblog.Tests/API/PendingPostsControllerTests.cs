using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Zemblog.API.Controllers;
using Zemblog.Domain;
using ZemBlog.Data.Interfaces;

namespace Zemblog.Tests.API
{
    public class PostsControllerTests
    {

        [Fact]
        public async void GetPendingPosts()
        {
            //setup
            var mockPostRepository = new Mock<IPostRepository>();

            var fakePosts = new List<Post>()
            {
                new Post
                {
                    Id = 1,
                    Author = "author1",
                    Title = "Post1",
                    Text = "post 1 content",
                    PostStatus = PostStatus.PendingApproval
                },
                new Post
                {
                    Id = 2,
                    Author = "author1",
                    Title = "Post2",
                    Text = "post 2 content",
                    PostStatus = PostStatus.Approved
                },
                new Post
                {
                    Id = 3,
                    Author = "author2",
                    Title = "Post3",
                    Text = "post 3 content",
                    PostStatus = PostStatus.PendingApproval
                }
             };

            mockPostRepository.Setup(m => m.GetPostsByStatus(PostStatus.PendingApproval))
                        .ReturnsAsync(fakePosts.Where(p => p.PostStatus == PostStatus.PendingApproval).ToList());

            //act
            var controller = new PendingPostsController(mockPostRepository.Object);
            var result = await controller.Get();
            var okResult = result as OkObjectResult;

            //Assert
            Assert.True(okResult is OkObjectResult);
            Assert.Equal(2, (okResult.Value as List<Post>).Count);            
        }

        [Fact]
        public async void ApprovePost()
        {
            //setup
            var mockPostRepository = new Mock<IPostRepository>();

            mockPostRepository.Setup(m => m.UpdatePostStatus(1, PostStatus.Approved))
                        .ReturnsAsync(1);

            //act
            var controller = new PendingPostsController(mockPostRepository.Object);
            var result = await controller.Post(1, "1");
            var okResult = result as OkObjectResult;

            //Assert
            Assert.True(okResult is OkObjectResult);
            Assert.Equal(1, Convert.ToInt32(okResult.Value));
        }
        
        [Fact]
        public async void RejectPost()
        {
            //setup
            var mockPostRepository = new Mock<IPostRepository>();

            mockPostRepository.Setup(m => m.UpdatePostStatus(1, PostStatus.Rejected))
                        .ReturnsAsync(1);

            //act
            var controller = new PendingPostsController(mockPostRepository.Object);
            var result = await controller.Post(1, "2");
            var okResult = result as OkObjectResult;

            //Assert
            Assert.True(okResult is OkObjectResult);
            Assert.Equal(1, Convert.ToInt32(okResult.Value));
        }
    }
}
