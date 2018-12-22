using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using MvcBlogApi.Models.ErrorModels;
using Newtonsoft.Json;

namespace MvcBlogApi.Methods
{
    public class ErrorMethods
    {
        public static Error Handle404()
        {
            Error err = new Error
            {
                ErrorCode = HttpStatusCode.NotFound,
                Message = "Not found"
            };
            

           
            return err;
        }

        public static Error Handle500()
        {
            Error err = new Error
            {
                ErrorCode = HttpStatusCode.InternalServerError,
                Message = "Internal error.Try later"
            };
            return err;
        }
    }
}