using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BlogModel;
using MvcBlogApi.Filters;
using MvcBlogApi.Models;
using Newtonsoft.Json;

namespace MvcBlogApi.Controllers.User
{
   
    public class ProfileController:ApiController
    {
        [BasicAuthentication]
        public JsonResult GetProfile(int id)
        {
            UserModel profile = Methods.GetMethods.GetUserProfile(id);
            if (profile == null)
            {
                var err = Methods.ErrorMethods.Handle404();
                JsonConvert.SerializeObject(err);
                return new JsonResult()
                {
                    Data = err,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }

            JsonResult jsonProfile = new JsonResult
            {
                Data = profile,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                ContentType = "application/json"
            };
            return jsonProfile;
        }

    }

   
}