using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcBlog.Filters
{
    public class RequireRequestValue : ActionMethodSelectorAttribute
    {
        public RequireRequestValue(string valueName)
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