using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Zemblog.Domain;
using ZemBlog.Data.Interfaces;
using ZemBlogMVC.Controllers;

namespace Zemblog.Tests.MVC
{
    public class PostsControllerTests
    {
        List<Post> fakePosts = new List<Post>()
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


        [Fact]
        public async void GetPendingPosts()
        {
            //setup
            var mockPostRepository = new Mock<IPostRepository>();

            mockPostRepository.Setup(m => m.GetPostsByStatus(PostStatus.PendingApproval))
                        .ReturnsAsync(fakePosts.Where(p => p.PostStatus == PostStatus.PendingApproval).ToList());

            //act
            var controller = new PostsController(mockPostRepository.Object);
            var result = await controller.Index("true");
            var viewResult = result as ViewResult;

            //Assert
            Assert.True(viewResult is ViewResult);
            Assert.Equal(2, ((List<Post>)viewResult.Model).Count);
        }

        [Fact]
        public async void GetApprovedPosts()
        {
            //setup
            var mockPostRepository = new Mock<IPostRepository>();

            mockPostRepository.Setup(m => m.GetPostsByStatus(PostStatus.Approved))
                        .ReturnsAsync(fakePosts.Where(p => p.PostStatus == PostStatus.Approved).ToList());

            //act
            var controller = new PostsController(mockPostRepository.Object);
            var result = await controller.Index();
            var viewResult = result as ViewResult;

            //Assert
            Assert.True(viewResult is ViewResult);
            Assert.Single(((List<Post>)viewResult.Model));
        }

        [Fact]
        public async void GetPostDetails()
        {
            var expected = new Post
            {
                Id = 1,
                Author = "author1",
                Title = "Post1",
                Text = "post 1 content"
            };

            //setup
            var mockPostRepository = new Mock<IPostRepository>();

            mockPostRepository.Setup(m => m.GetPost(1))
                        .ReturnsAsync(expected);

            //act
            var controller = new PostsController(mockPostRepository.Object);
            var result = await controller.Details(1);
            var viewResult = result as ViewResult;

            //Assert
            Assert.True(viewResult is ViewResult);
            Assert.Equal(expected, viewResult.Model);
        }

        [Fact]
        public async void CreatePost()
        {
            var post = new Post
            {
                Id = 1,
                Author = "author1",
                Title = "Post1",
                Text = "post 1 content"
            };

            //setup
            var mockPostRepository = new Mock<IPostRepository>();

            mockPostRepository.Setup(m => m.CreatePost(post))
                        .ReturnsAsync(1);

        //act
        var controller = new PostsController(mockPostRepository.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            
            var result = await controller.Create(post);
            var viewResult = result as RedirectToActionResult;

            //Assert
            Assert.True(viewResult is RedirectToActionResult);
            Assert.Equal("MyPosts", viewResult.ActionName);
        }
    }
}