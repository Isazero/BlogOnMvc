using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlogModel;
using MvcBlog.Filters;
namespace MvcBlog.Admin
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                ViewBag.Users = db.Users.ToList();
                return View("~/Views/Admin/Index.cshtml");
            }
        }

        public ActionResult OpenCreateUser()
        {
            return View("~/Views/Admin/CreateUser.cshtml");
        }

        public ActionResult CreateUser(string name, string surname, string email, string password)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                User user = new User();
                user.Name = name;
                user.Surname = surname;
                user.Email = email;
                user.Password = Methods.PasswordMethods.CreateMD5(password);
                db.Users.Add(user);
                db.SaveChanges();
                
                return View("~/Views/Admin/Index.cshtml");
            }
        }
        public ActionResult UpdateUser(int userId, string name, string surname, string email, string password,bool isDeleted = false)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                User user = db.Users.FirstOrDefault(u=>u.UserId==userId);
                if (user != null)
                {
                    user.Name = name;
                    user.Surname = surname;
                    user.Email = email;
                    if (!string.IsNullOrEmpty(password))
                    {
                        user.Password = Methods.PasswordMethods.CreateMD5(password);
                    }

                    user.IsDeleted = Convert.ToBoolean(isDeleted);
                    if (!isDeleted)
                    {
                        var posts = db.Posts.Where(p => p.UserId == userId).ToList();
                        RestoreUserPosts(posts);
                    }
                }

                db.SaveChanges();

                return View("~/Views/Admin/Index.cshtml");
            }
        }

        private void RestoreUserPosts(List<Post> posts)
        {
            foreach (var p in posts)
            {
                p.IsDeleted = false;
            }
        }

        public ActionResult OpenUpdateUser(int userId)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                User user = db.Users.FirstOrDefault(u => u.UserId == userId);
                ViewBag.User = user;
                return View("~/Views/Admin/CreateUser.cshtml");
            }
        }

        public ActionResult DeleteUser(int userId)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                User user = db.Users.FirstOrDefault(u=>u.UserId==userId);
                if (user != null) user.IsDeleted = true;
                List<Post> posts = db.Posts.Where(p => p.UserId == userId).ToList();
                
                foreach (var p in posts)
                {
                    p.IsDeleted = true;
                }

                List<Comment> comments = db.Comments.ToList();
                foreach (var c in comments)
                {
                    int postId = posts.Where(p => p.PostId == c.PostId).Select(p=>p.PostId).FirstOrDefault();
                    if (c.UserId == userId)
                    {
                        db.Comments.Remove(c);
                    }
                    else if(c.PostId== postId)
                    {
                       db.Comments.Remove(c);
                    }

                }
                db.SaveChanges();
                ViewBag.Users = db.Users.ToList();
                return View("~/Views/Admin/Index.cshtml");
            }
            
        }

        public ActionResult Sort(string sortOrder)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                List<User> users = db.Users.ToList();
                switch (sortOrder)
                {
                    case "id_desc":
                        users = new List<User>(users.OrderByDescending(u => u.UserId));
                            
                        break;
                    case "name_desc":
                        users = new List<User>(users.OrderByDescending(u => u.Name));
                        break;
                    case "surname_desc":
                        users = new List<User> (users.OrderByDescending(u => u.Surname));
                        break;
                    case "isDeleted_desc":
                        users = new List<User>( users.OrderByDescending(u => u.IsDeleted));
                        break;
                    default:
                        users = new List<User> (users.OrderBy(u => u.Name));
                        break;
                }

                ViewBag.Users = users;
                return View("~/Views/Admin/Index.cshtml");
            }
           

        }


        public ActionResult Search(string searchVal,int filterVal=0)
        {
            using (var db = new BlogDbContext())
            {
                List<User> users = db.Users.ToList();
                switch (filterVal)
                {
                    case 1:
                        users =  users.Where(u => u.UserId.ToString().Contains(searchVal)).ToList();
                        break;
                    case 2:
                        users = users.Where(u => u.Name.Contains(searchVal)).ToList();
                        break;
                    case 3:
                        users = users.Where(u => u.Surname.Contains(searchVal)).ToList();
                        break;
                    case 4:
                        users = users.Where(u => u.Email.Contains(searchVal)).ToList();
                        break;
                    case 5:
                        users = users.Where(u => u.Password.Contains(searchVal)).ToList();
                        break;
                    case 6:
                        users = users.Where(u => u.IsDeleted.ToString().Contains(searchVal)).ToList();
                        break;
                    default:
                        users = users.Where(u =>
                            u.Name.Contains(searchVal) || u.Surname.Contains(searchVal) ||
                            u.UserId.ToString().Contains(searchVal) || u.Email.Contains(searchVal) ||
                            u.IsDeleted.ToString().Contains(searchVal) || u.Password.Contains(searchVal)).ToList();

                        break;
                }
                ViewBag.Users = users;
                return View("~/Views/Admin/Index.cshtml");
            }
        }

    }
}
