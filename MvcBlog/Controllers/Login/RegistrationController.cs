using System.Web.Mvc;
using System.Web.Security;
using Common;
using MvcBlog.Methods;
using MvcBlog.Models;

namespace MvcBlog.Controllers.Login
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult OpenRegistrationForm()
        {
            return View("~/Views/Register/Registration.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var status = InsertMethods.RegisterNewUser(model);
                if (status == (int)Registration.Success && CheckMethods.IsUserExists(model.Login))
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Account");
                }
            }
            ViewBag.Error = "Already exists";
            return View("~/Views/Register/Registration.cshtml", model);
        }
    }
}