using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MvcBlogApi.Methods;

namespace MvcBlogApi.Filters
{
   
        public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
        {
            public override void OnAuthorization(HttpActionContext actionContext)
            {
                var authHeader = actionContext.Request.Headers.Authorization;

                if (authHeader != null)
                {
                    var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                    var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                    var usernamePasswordArray = decodedAuthenticationToken.Split(':');
                    var userName = usernamePasswordArray[0];
                    var password = usernamePasswordArray[1];

                   var isValid = CheckMethods.CheckIsUserExist(userName,password);

                    if (isValid)
                    {
                        var principal = new GenericPrincipal(new GenericIdentity(userName), null);
                        Thread.CurrentPrincipal = principal;
                    

                        return;
                    }
                }

                HandleUnathorized(actionContext);
            }

            private static void HandleUnathorized(HttpActionContext actionContext)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Data' location = 'http://localhost:");
            }
        }
    
}