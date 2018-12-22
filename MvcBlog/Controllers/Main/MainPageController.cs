using System.Linq;
using System.Web.Mvc;
using BlogModel;
using MvcBlog.Methods;

namespace MvcBlog.Controllers
{
    public class MainPageController : Controller
    {
        // GET: MainPage
        public ActionResult Index()
        {
            using (var db = new BlogDbContext())
            {
                var posts = db.Posts.Where(p => p.IsDeleted == false).OrderByDescending(p => p.PublishDate)
                    .Take(10).ToList();
                posts = GetMethods.GetInitializedUserList(posts);
                ViewBag.Posts = posts;
                ViewData["Layout"] = GetMethods.GetLayout(User.Identity.Name);


                return View("~/Views/Main/Index.cshtml");
            }
        }


        public ActionResult ShowAll()
        {
            using (var db = new BlogDbContext())
            {
                var posts = db.Posts.Where(p => p.IsDeleted == false).OrderByDescending(p => p.PublishDate)
                    .ToList();
                posts = GetMethods.GetInitializedUserList(posts);
                ViewBag.Posts = posts;
                ViewData["Layout"] = GetMethods.GetLayout(User.Identity.Name);

                return View("~/Views/Main/AllPosts.cshtml");
            }
        }

        public ActionResult ShowPost(int postId)
        {
            using (var db = new BlogDbContext())
            {
                var post = db.Posts.FirstOrDefault(p => p.PostId == postId);
                var user = db.Users.FirstOrDefault(u => u.UserId == post.UserId);
                var comments = db.Comments.Where(c => c.PostId == postId).ToList();
                comments = GetMethods.GetInitializedUserList(comments);
                ViewBag.User = user;

                ViewBag.Comments = comments;
                ViewData["Layout"] = GetMethods.GetLayout(User.Identity.Name);
                return View("~/Views/Main/ShowPost.cshtml", post);
            }
        }
    }
}