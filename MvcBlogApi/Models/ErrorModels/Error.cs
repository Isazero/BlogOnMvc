using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Services.Description;

namespace MvcBlogApi.Models.ErrorModels
{
    public class Error
    {
        public HttpStatusCode ErrorCode { get; set; }
         public string Message { get; set; }
    }
}