using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BlogModel;
using MvcBlog.Filters;
using MvcBlog.Methods;
using MvcBlog.Models;

namespace MvcBlog.Controllers.Admin
{
    public class PostController : Controller
    {
        // GET: Post
        [AdminAccess]
        public ActionResult Index()
        {
            using (var db = new BlogDbContext())
            {
                ViewBag.Posts = db.Posts.ToList();
                return View("~/Views/Admin/AdminMain.cshtml");
            }
        }

        [AdminAccess]
        public ActionResult Sort(string sortOrder)
        {
            using (var db = new BlogDbContext())
            {
                var posts = db.Posts.ToList();
                switch (sortOrder)
                {
                    case "id_desc":
                        posts = new List<Post>(posts.OrderByDescending(u => u.PostId));
                        break;
                    case "title_desc":
                        posts = new List<Post>(posts.OrderByDescending(u => u.Title));
                        break;
                    case "content_desc":
                        posts = new List<Post>(posts.OrderByDescending(u => u.Content));
                        break;
                    case "user_desc":
                        posts = new List<Post>(posts.OrderByDescending(u => u.UserId));
                        break;
                    case "date_desc":
                        posts = new List<Post>(posts.OrderByDescending(u => u.PublishDate));
                        break;
                    case "slug_desc":
                        posts = new List<Post>(posts.OrderByDescending(u => u.Slug));
                        break;
                    case "isDeleted_desc":
                        posts = new List<Post>(posts.OrderByDescending(u => u.IsDeleted));
                        break;
                    default:
                        posts = new List<Post>(posts.OrderBy(u => u.Title));
                        break;
                }

                ViewBag.Posts = posts;
                return View("~/Views/Admin/AdminMain.cshtml");
            }
        }

        [AdminAccess]
        public ActionResult Search(string searchVal, DateTime? date, int filterVal = 0)
        {
            using (var db = new BlogDbContext())
            {
                var posts = db.Posts.ToList();
                switch (filterVal)
                {
                    case 1:
                        posts = posts.Where(p =>
                            p.PostId.ToString().Contains(searchVal)).ToList();
                        break;
                    case 2:
                        posts = posts.Where(p =>
                            p.Title.Contains(searchVal)).ToList();

                        break;
                    case 3:
                        posts = posts.Where(p =>
                            p.Slug.Contains(searchVal)).ToList();
                        break;
                    case 4:
                        posts = posts.Where(p =>
                            p.Content.Contains(searchVal)).ToList();
                        break;
                    case 5:
                        posts = posts.Where(p =>
                            p.UserId.ToString().Contains(searchVal)).ToList();
                        break;
                    case 6:
                        posts = posts.Where(p =>
                            p.PublishDate.ToString(CultureInfo.InvariantCulture).Contains(searchVal)).ToList();
                        break;
                    case 7:
                        posts = posts.Where(p =>
                            p.IsDeleted.ToString().Contains(searchVal)).ToList();
                        break;
                    default:
                        posts = posts.Where(p =>
                            p.PostId.ToString().Contains(searchVal) || p.Content.Contains(searchVal) ||
                            p.Slug.Contains(searchVal) || p.Title.Contains(searchVal) ||
                            p.UserId.ToString().Contains(searchVal) || p.IsDeleted.ToString().Contains(searchVal) ||
                            p.PublishDate.ToString(CultureInfo.InvariantCulture).Contains(searchVal)).ToList();
                        break;
                }

                ViewBag.Posts = posts;
                return View("~/Views/Admin/AdminMain.cshtml");
            }
        }

        [AdminAccess]
        public ActionResult OpenCreatePost()
        {
            return View("~/Views/Admin/CreatePost.cshtml");
        }

        [AdminAccess]
        public ActionResult CreatePost(Post post)
        {
            if (string.IsNullOrEmpty(post.PublishDate.ToString(CultureInfo.CurrentCulture))) post.PublishDate = DateTime.Now;
           
            using (var db = new BlogDbContext())
            {
                try
                {
                    db.Posts.Add(post);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    // ignored
                }

                return View("~/Views/Admin/AdminMain.cshtml");
            }
        }

        [AdminAccess]
        public ActionResult OpenUpdatePost(int postId)
        {
            using (var db = new BlogDbContext())
            {
                var post = db.Posts.FirstOrDefault(p => p.PostId == postId);
                ViewBag.Post = post;
                return View("~/Views/Admin/CreatePost.cshtml");
            }
        }

        [AdminAccess]
        public ActionResult UpdatePost(Post post)
        {
            string wrongDate = "01/01/0001";
            if (string.IsNullOrEmpty(post.PublishDate.ToString(CultureInfo.CurrentCulture)) || post.PublishDate == Convert.ToDateTime(wrongDate)) post.PublishDate = DateTime.Now;

            using (var db = new BlogDbContext())
            {
                try
                {
                    var postInDb = db.Posts.FirstOrDefault(u => u.PostId == post.PostId);
                    if (postInDb != null)
                    {
                        postInDb.Title = post.Title;
                        postInDb.Slug = post.Slug;
                        postInDb.Content = post.Content;
                        postInDb.UserId = post.UserId;
                        postInDb.PublishDate = post.PublishDate;
                        postInDb.IsDeleted = post.IsDeleted;
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    // ignored
                }

                return View("~/Views/Admin/AdminMain.cshtml");
            }
        }

        [Filters.Authorize]
        public void DeletePost(int postId)
        {
            using (var db = new BlogDbContext())
            {
                var post = db.Posts.FirstOrDefault(p => p.PostId == postId);
                if (post != null) post.IsDeleted = true;
                try
                {
                    var comments = db.Comments.Where(c => c.PostId == postId).ToList();
                    foreach (var c in comments) db.Comments.Remove(c);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    //ignored
                }
 
            }
        }


        [Filters.Authorize]
        public ActionResult OpenAddPost()
        {
            ViewData["Layout"] = GetMethods.GetLayout(User.Identity.Name);
            return View("~/Views/User/NewPost.cshtml");
        }

        [Filters.Authorize]
        public ActionResult AddPost(PostModel post)
        {
            try
            {
                InsertMethods.AddNewPost(post, User.Identity.Name);
            }
            catch (Exception e)
            {
                // ignored
            }
            return RedirectToAction("Index", "MainPage");
        }

        [Filters.Authorize]
        public ActionResult UserDeletePost(int postId)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                var username = User.Identity.Name;
                if (CheckMethods.IsUsersPost(postId,username))
                {
                    DeletePost(postId);
                    return RedirectToAction("Profile", "Account");
                }
            }
            return RedirectToAction("AccessDenied","Error");
        }

        [Filters.Authorize]
        public ActionResult UserUpdatePost(int postId,string title, string content)
        {

            using (BlogDbContext db = new BlogDbContext())
            {
                var username = User.Identity.Name;
                if (CheckMethods.IsUsersPost(postId, username))
                {
                    InsertMethods.UpdatePost(postId,title,content);
                    return RedirectToAction("ShowPost", "MainPage", new {postId});
                }
            }

            return RedirectToAction("AccessDenied","Error");
        }

        [Filters.Authorize]
        public ActionResult OpenUserUpdatePost(int postid)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                var username = User.Identity.Name;
                if (CheckMethods.IsUsersPost(postid, username))
                {
                    ViewData["Layout"] = GetMethods.GetLayout(User.Identity.Name);
                    var post = db.Posts.FirstOrDefault(p => p.PostId == postid);
                    return View("~/Views/User/UserUpdatePost.cshtml",post);
                }

                return RedirectToAction("AccessDenied", "Error");
            }
        }
    }
}