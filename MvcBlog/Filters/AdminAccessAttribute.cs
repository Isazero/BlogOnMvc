using System.Web;
using System.Web.Mvc;
using MvcBlog.Methods;

namespace MvcBlog.Filters
{
    public class AdminAccessAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAdmin = CheckMethods.IsCurrentUserAdmin(httpContext.User.Identity.Name);
            if (httpContext.Request.IsAuthenticated && isAdmin) return true;

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var urlHelper = new UrlHelper(filterContext.RequestContext);
            if (!filterContext.RequestContext.HttpContext.User.Identity.IsAuthenticated)
                filterContext.Result = new RedirectResult(urlHelper.Action("AccessDenied", "Error"));
        }
    }
}