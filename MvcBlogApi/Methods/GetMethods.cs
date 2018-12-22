using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using BlogModel;
using MvcBlogApi.Models;
using Newtonsoft.Json;

namespace MvcBlogApi.Methods
{
    public class GetMethods
    {
        public static List<PostModel> GetAllPosts()
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                List<Post> posts = db.Posts.Where(p => p.IsDeleted != true).ToList();
                List<PostModel> postModels = new List<PostModel>();
                foreach (var p in posts)
                {
                   postModels.Add(new PostModel
                    {
                        PostId = p.PostId,
                        PublishDate = p.PublishDate,
                        IsDeleted = p.IsDeleted,
                        Slug = p.Slug,
                        UserId = p.UserId,
                        Title = p.Title,
                        Content = p.Content,
                        Comments = GetListOfComments(p.PostId)
                    });
                }
                return postModels;
            }

        }

        private static ICollection<CommentModel> GetListOfComments(int postId)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                List<Comment> comments = db.Comments.Where(c => c.PostId == postId).ToList();
                List<CommentModel> commentModels = new List<CommentModel>();
                foreach (var c in comments)
                {
                    commentModels.Add(new CommentModel()
                    {
                        Content = c.Content,
                        PostId = c.PostId,
                        UserId = c.UserId,
                        DateCreated = c.DateCreated,
                        CommentId = c.CommentId
                    });
                }

                return commentModels;
            }
        }

        public static PostModel GetPost(int postId)
        {
            using (BlogDbContext db = new BlogDbContext())
            {

                var post = db.Posts.FirstOrDefault(p => p.PostId == postId && p.IsDeleted==false);
                if (post != null && !post.IsDeleted)
                {
                    var postModel= new PostModel
                    {
                        PostId = post.PostId,
                        PublishDate = post.PublishDate,
                        IsDeleted = post.IsDeleted,
                        Slug = post.Slug,
                        UserId = post.UserId,
                        Title = post.Title,
                        Content = post.Content,
                        Comments = GetListOfComments(postId)
                    };
                    return postModel;
                }
            }

            return null;
        }

        public static UserModel GetUserProfile(int id)
        {
            using (BlogDbContext db= new BlogDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserId == id);
                if (user != null && user.IsDeleted != true)
                {
                    UserModel profile = new UserModel()
                    {
                        Posts = GetAllUserPosts(user.UserId),
                        Username = user.Username,
                        Name = user.Name,
                        Surname = user.Surname,
                        Email = user.Email,
                        Comments = GetAllUserComments(user.UserId)
                    };
                    return profile;
                }

                return null;
            }
        }

        private static ICollection<CommentModel> GetAllUserComments(int userId)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                List<Comment> comments = db.Comments.Where(c => c.UserId == userId).ToList();
                List<CommentModel> commentModels = new List<CommentModel>();
                foreach (var c in comments)
                {
                    commentModels.Add(new CommentModel()
                    {
                        Content = c.Content,
                        PostId = c.PostId,
                        UserId = c.UserId,
                        DateCreated = c.DateCreated,
                        CommentId = c.CommentId
                    });
                }

                return commentModels;
            }
        }

        private static ICollection<PostModel> GetAllUserPosts(int userId)
        {
            using (BlogDbContext db =new BlogDbContext())
            {
                List<PostModel> postModels = new List<PostModel>();
                var posts = db.Posts.Where(p => p.UserId==userId && p.IsDeleted == false).ToList();
                foreach (var post in posts)
                {
                    if (!post.IsDeleted)
                    {
                        var postModel = new PostModel
                        {
                            PostId = post.PostId,
                            PublishDate = post.PublishDate,
                            IsDeleted = post.IsDeleted,
                            Slug = post.Slug,
                            UserId = post.UserId,
                            Title = post.Title,
                            Content = post.Content,
                            Comments = GetListOfComments(post.PostId)
                        };
                        postModels.Add(postModel);
                    }
                }

                return postModels;
            }
        }

        public static int GetUserRoleId(string username)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                int userId = db.Users.Where(u => u.Username.Equals(username)).Select(u => u.RoleId).FirstOrDefault();
                return userId;
            }
        }

        public static UserModel GetUserInformationByBase64(string userToken)
        {
            string username = DecodeHeader(userToken);
            using (BlogDbContext db = new BlogDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user != null && !user.IsDeleted)
                {
                    UserModel userModel = new UserModel()
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        Name = user.Name,
                        Surname = user.Surname,
                        IsDeleted = user.IsDeleted,
                        Email = user.Email,
                        RoleId = user.RoleId,

                    };
                    return userModel;
                }
            }

            return null;
        }

        private static string DecodeHeader(string user)
        {
            var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(user));
            var usernamePasswordArray = decodedAuthenticationToken.Split(':');
            var userName = usernamePasswordArray[0];
            var password = usernamePasswordArray[1];
            return userName ;
        }
    }
}