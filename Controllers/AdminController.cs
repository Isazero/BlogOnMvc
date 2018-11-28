using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Management;
using System.Web.Mvc;
using BlogModel;

namespace MvcBlog.Controllers
{
    public class AdminController : Controller
    {

        // GET: Blog
        
        public ActionResult Index()
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                ViewBag.Users = db.Users.ToList();
                //ViewBag.Comments = db.Comments.ToList();
                //ViewBag.Posts = db.Posts.ToList();
                return View();
            }
        }

        [RequireRequestValue("sortOrder")]
        public ActionResult Index(string sortOrder, string table)
        {
            using (var db = new BlogDbContext())
            {
                var users = from u in db.Users
                    select u;
                var posts = from p in db.Posts
                    select p;
                var comments = from c in db.Comments
                    select c;
                if (table.ToLower() == "user")
                {
                    switch (sortOrder)
                    {
                        case "id_desc":
                            users = users.OrderByDescending(u => u.UserId);
                            break;
                        case "name_desc":
                            users = users.OrderByDescending(u => u.Name);
                            break;
                        case "surname_desc":
                            users = users.OrderByDescending(u => u.Surname);
                            break;
                        case "isDeleted_desc":
                            users = users.OrderByDescending(u => u.IsDeleted);
                            break;
                        default:
                            users = users.OrderBy(u => u.Name);
                            break;
                    }
                }
                else if (table.ToLower() == "post")
                {
                    switch (sortOrder)
                    {
                        case "id_desc":
                            posts = posts.OrderByDescending(u => u.PostId);
                            break;
                        case "title_desc":
                            posts = posts.OrderByDescending(u => u.Title);
                            break;
                        case "content_desc":
                            posts = posts.OrderByDescending(u => u.Content);
                            break;
                        case "user_desc":
                            posts = posts.OrderByDescending(u => u.UserId);
                            break;
                        case "date_desc":
                            posts = posts.OrderByDescending(u => u.PublishDate);
                            break;
                        case "slug_desc":
                            posts = posts.OrderByDescending(u => u.Slug);
                            break;
                        case "isDeleted_desc":
                            posts = posts.OrderByDescending(u => u.IsDeleted);
                            break;
                        default:
                            posts = posts.OrderBy(u => u.Title);
                            break;
                    }
                }
                else if (table.ToLower() == "comment")
                {
                    switch (sortOrder)
                    {
                        case "id_desc":
                            comments = comments.OrderByDescending(u => u.CommentId);
                            break;
                        case "content_desc":
                            comments = comments.OrderByDescending(u => u.Content);
                            break;
                        case "userId_desc":
                            comments = comments.OrderByDescending(u => u.UserId);
                            break;
                        case "postId_desc":
                            comments = comments.OrderByDescending(u => u.PostId);
                            break;
                        case "date_desc":
                            comments = comments.OrderByDescending(u => u.DateCreated);
                            break;
                        default:
                            comments = comments.OrderBy(u => u.CommentId);
                            break;
                    }
                    
                }

                ViewBag.Users = users.ToList();
                ViewBag.Comments = comments.ToList();
                ViewBag.Posts = posts.ToList();
                return View();
            }
        }

        [RequireRequestValue("tab")]
        public ActionResult Index(int tab,string searchVal)
        {
            using (var db = new BlogDbContext())
            {
                List<User> users = db.Users.ToList();
                List<Post> posts = db.Posts.ToList();
                List<Comment> comments = db.Comments.ToList();
                if (tab == 1)
                {
                    users = users.Where(u =>
                        u.Name.Contains(searchVal) || u.Surname.Contains(searchVal) ||
                        u.UserId.ToString().Contains(searchVal) || u.Email.Contains(searchVal) ||
                        u.IsDeleted.ToString().Contains(searchVal) || u.Password.Contains(searchVal)).ToList();
                }
                else if (tab == 2)
                {
                    posts = posts.Where(p =>
                        p.PostId.ToString().Contains(searchVal) || p.Content.Contains(searchVal) ||
                        p.Slug.Contains(searchVal) || p.Title.Contains(searchVal) ||
                        p.UserId.ToString().Contains(searchVal) || p.IsDeleted.ToString().Contains(searchVal) ||
                        p.PublishDate.ToString(CultureInfo.InvariantCulture).Contains(searchVal)).ToList();
                }
                else if (tab == 3)
                {
                    comments = comments.Where(c =>
                        c.CommentId.ToString().Contains(searchVal) || c.Content.Contains(searchVal) ||
                        c.DateCreated.ToString(CultureInfo.InvariantCulture).Contains(searchVal) || c.PostId.ToString().Contains(searchVal) ||
                        c.UserId.ToString().Contains(searchVal)).ToList();
                }

                ViewBag.Users = users.ToList();
                ViewBag.Comments = comments.ToList();
                ViewBag.Posts = posts.ToList();
                return View();

            }


        }



        public class RequireRequestValueAttribute : ActionMethodSelectorAttribute
        {
            public RequireRequestValueAttribute(string valueName)
            {
                ValueName = valueName;
            }
            public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
            {
                return (controllerContext.HttpContext.Request[ValueName] != null);
            }
            public string ValueName { get; }
        }
    }
}
