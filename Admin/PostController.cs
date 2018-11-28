using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using BlogModel;

namespace MvcBlog.Admin
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                ViewBag.Posts = db.Posts.ToList();
                return View("~/Views/Admin/Index.cshtml");
            }
        }

        public ActionResult Sort(string sortOrder)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                List<Post> posts = db.Posts.ToList();
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
                return View("~/Views/Admin/Index.cshtml");
            }
        }


        public ActionResult Search(string searchVal, DateTime? date, int filterVal = 0)
        {

            using (var db = new BlogDbContext())
            {
                List<Post> posts = db.Posts.ToList();
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
                return View("~/Views/Admin/Index.cshtml");
            }
        }

        public ActionResult OpenCreatePost()
        {
            return View("~/Views/Admin/CreatePost.cshtml");
        }

        public ActionResult CreatePost(string title, string slug, string content, int userId, DateTime publishDate,
            bool isDeleted = false)
        {
            if (string.IsNullOrEmpty(publishDate.ToString(CultureInfo.CurrentCulture)))
            {
                publishDate = DateTime.Today;
            }

            using (BlogDbContext db = new BlogDbContext())
            {
                try
                {
                    Post post = new Post();
                    post.Title = title;
                    post.Slug = slug;
                    post.Content = content;
                    post.UserId = userId;
                    post.PublishDate = publishDate;
                    post.IsDeleted = isDeleted;
                    db.Posts.Add(post);
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                }
                return View("~/Views/Admin/Index.cshtml");
            }
        }
        public ActionResult OpenUpdatePost(int postId)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                Post post = db.Posts.FirstOrDefault(p => p.PostId == postId);
                ViewBag.Post = post;
                return View("~/Views/Admin/CreatePost.cshtml");
            }
        }
        public ActionResult UpdatePost(int postId, string title, string slug, string content, int userId,
            DateTime publishDate, bool isDeleted = false)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                try
                {
                    Post post = db.Posts.FirstOrDefault(u => u.PostId == postId);
                    if (post != null)
                    {
                        post.Title = title;
                        post.Slug = slug;
                        post.Content = content;
                        post.UserId = userId;
                        post.PublishDate = publishDate;
                        post.IsDeleted = isDeleted;
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                }
                return View("~/Views/Admin/Index.cshtml");
            }
        }

        public ActionResult DeletePost(int postId)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                Post post = db.Posts.FirstOrDefault(p => p.PostId == postId);
                if (post != null) post.IsDeleted = true;
                try
                {
                    List<Comment> comments = db.Comments.Where(c => c.PostId == postId).ToList();
                    foreach (var c in comments)
                    {
                        db.Comments.Remove(c);
                    }
                }
                catch (Exception e)
                {

                }
                db.SaveChanges();
                ViewBag.Posts = db.Posts.ToList();
            }
            return View("~/Views/Admin/Index.cshtml");
        }
    }
}