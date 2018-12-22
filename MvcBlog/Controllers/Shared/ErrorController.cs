using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MvcBlog.Controllers.Shared
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult AccessDenied()
        {
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }
    }
}