using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Zemblog.Domain;
using ZemBlog.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Zemblog.Domain.Constants;

namespace ZemBlogMVC.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private IPostRepository postRepository;

        public PostsController(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string pendingOnly = "false")
        {
            List<Post> data;

            if (pendingOnly == "true")
                data = await postRepository.GetPostsByStatus(PostStatus.PendingApproval);
            else
                data = await postRepository.GetPostsByStatus(PostStatus.Approved);

            ViewBag.PendingOnly = bool.Parse(pendingOnly);
            return View(data);
        }

        [Authorize(Roles = Constants.WriterRole)]
        public async Task<IActionResult> MyPosts()
        {
            return View(await postRepository.GetPosts(User.Identity.Name));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var post = await postRepository.GetPost(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [Authorize(Roles = Constants.WriterRole)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Constants.WriterRole)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Text,PublishedDate,PostStatus")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Author = User.Identity.Name;
                await postRepository.CreatePost(post);
                return RedirectToAction(nameof(MyPosts));
            }

            return View(post);
        }

        public async Task<IActionResult> UpdatePostStatus(int id, string status)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var newSPostStatus = (PostStatus)Enum.Parse(typeof(PostStatus), status);

            await postRepository.UpdatePostStatus(id, newSPostStatus);
            
            if (User.IsInRole(Constants.EditorRole))
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(MyPosts));
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddComment(int postId, Comment comment)
        {
            if (postId == 0)
            {
                return NotFound();
            }

            comment.User = User.Identity.Name ?? "guest";

            await postRepository.AddComment(postId, comment);

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = Constants.WriterRole)]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var post = await postRepository.GetPost(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        [Authorize(Roles = Constants.WriterRole)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Text")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await postRepository.UpdatePost(id, post);
                return RedirectToAction(nameof(MyPosts));
            }
            return View(post);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var post = await postRepository.GetPost(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await postRepository.DeletePost(id);
            if (User.IsInRole("editor"))
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(MyPosts));
        }
    }
}