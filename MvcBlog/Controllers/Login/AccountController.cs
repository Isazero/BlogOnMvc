using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BlogModel;
using MvcBlog.Methods;
using MvcBlog.Models;

namespace MvcBlog.Controllers.Login
{
    public class AccountController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("~/Views/Login/Login.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, object returnurl)
        {
            if (ModelState.IsValid)
                if (CheckMethods.Authenticate(model.Login, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "MainPage");
                }
                else
                {
                    ViewBag.Error = "Пользователя с таким логином и паролем не существует";
                }

            return View("~/Views/Login/Login.cshtml", model);
        }

        [Filters.Authorize]
        public ActionResult Profile()
        {
            using (var db = new BlogDbContext())
            {
                var login = User.Identity.Name;
                var userId = db.Users.Where(u => u.Username.Equals(login)).Select(u => u.UserId).FirstOrDefault();
                var userPosts = db.Posts.Where(p => p.UserId == userId && p.IsDeleted == false)
                    .OrderByDescending(p => p.PublishDate).ToList();
                userPosts = GetMethods.GetInitializedUserList(userPosts);
                ViewData["Layout"] = GetMethods.GetLayout(login);
                return View("~/Views/User/Profile.cshtml", userPosts);
            }
        }
    }
}