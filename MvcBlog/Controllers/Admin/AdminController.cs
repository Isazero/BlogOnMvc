using System.Linq;
using System.Net;
using System.Web.Mvc;
using BlogModel;
using MvcBlog.Filters;

namespace MvcBlog.Controllers.Admin
{
    [AdminAccess]
    public class AdminController : Controller
    {

        // GET: Blog

        public ActionResult AdminMain()
        {

            using (BlogDbContext db = new BlogDbContext())
            {
               
                    ViewBag.Users = db.Users.ToList();
                    return View();
                
            }
            
        }
    }
}
