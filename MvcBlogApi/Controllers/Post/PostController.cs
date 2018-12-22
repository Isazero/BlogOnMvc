using System;
using System.Web.Http;
using System.Web.Mvc;
using BlogModel;
using MvcBlogApi.Filters;
using MvcBlogApi.Methods;
using Newtonsoft.Json;

namespace MvcBlogApi.Controllers.Post
{
    public class PostController : ApiController
    {
        public JsonResult GetAllPosts()
        {
            var allPosts = GetMethods.GetAllPosts();
            return new JsonResult
            {
                Data = allPosts,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [BasicAuthentication]
        public JsonResult GetPost(int id)
        {
            var post = GetMethods.GetPost(id);
            if (post == null)
            {
                var err = ErrorMethods.Handle404();
                JsonConvert.SerializeObject(err);
                return new JsonResult
                {
                    Data = err,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    ContentType = "application/json"
                };
            }

            var jsonResult = new JsonResult
            {
                Data = post,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                ContentType = "application/json"
            };
            return jsonResult;
        }

        [BasicAuthentication]
        [System.Web.Http.HttpPost]
        public JsonResult PostUserCreatePost(string title, string content)
        {
            var user = ActionContext.Request.Headers.Authorization.Parameter;
            var userInfo = GetMethods.GetUserInformationByBase64(user);
            var res = CreateNewPost(userInfo.UserId, title, content);
            if (res == 1)
                return new JsonResult
                {
                    Data = new{status="successful"}
                };

            var err = ErrorMethods.Handle500();
            JsonConvert.SerializeObject(err);
            return new JsonResult
            {
                Data = err,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                ContentType = "application/json"
            };
        }
       
        private int CreateNewPost(int userId, string title, string content)
        {
            using (var db = new BlogDbContext())
            {
                try
                {
                    var post = new BlogModel.Post
                    {
                        Title = title,
                        Slug = title.Trim(),
                        Content = content,
                        UserId = userId,
                        PublishDate = DateTime.Now,
                        IsDeleted = false
                    };
                    db.Posts.Add(post);
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return -1;
                }
            }

            return 1;
        }
    }
}