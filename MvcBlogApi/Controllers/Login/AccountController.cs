using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using MvcBlogApi.Methods;
using MvcBlogApi.Models.ErrorModels;
using Newtonsoft.Json;

namespace MvcBlogApi.Controllers.Login
{
    public class AccountController : ApiController
    {
       

        // GET: api/Account/5
        [System.Web.Http.HttpPost]
        public JsonResult PostRegisterUser(string username,string name,string surname,string password,string email)
        {
            InsertMethods.RegisterNewUser(username,name,surname,password,email);
            if (CheckMethods.IsUserExists(username))
            {
                JsonResult res = new JsonResult()
                {
                    Data = new { status="succesful"}
                };
                return res;
            }

            var err = new Error() {ErrorCode = HttpStatusCode.InternalServerError, Message = "Server error"};
            JsonConvert.SerializeObject(err);
            return new JsonResult()
            {
                Data = err
            };
        }
    }
}
