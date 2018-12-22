using System.Web.Mvc;
using System.Web.Security;
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
                InsertMethods.RegisterNewUser(model);
                if (CheckMethods.IsUserExists(model.Login))
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Account");
                }

                ModelState.AddModelError("", "Пользователь с таким логином уже существует");
            }

            return View("~/Views/Register/Registration.cshtml", model);
        }
    }
}