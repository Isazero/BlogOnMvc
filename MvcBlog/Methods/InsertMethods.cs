using System;
using System.Linq;
using BlogModel;
using Common;
using MvcBlog.Models;

namespace MvcBlog.Methods
{
    public class InsertMethods
    {
        public static int RegisterNewUser(RegisterModel model)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                if (db.Users.ToList().Count != 0)
                {
                    //Check if user exist
                    if (db.Users.Any(u => u.Username.Equals(model.Login)))
                    {
                        return (int)Registration.UserExists;
                    }
                }

                User newUser = new User
                {
                    Username = model.Login,
                    Password = GetMethods.GetHash(model.Password),
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    RoleId = 1,
                    IsDeleted = false
                };
                db.Users.Add(newUser);
                db.SaveChanges();
                return (int)Registration.Success;
            }

        }

        public static void AddNewPost(PostModel post,string login)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                var newPost = new Post
                {
                    Title = post.Title,
                    Slug = GetMethods.GetSlug(post.Title),
                    Content = post.Content,
                    UserId = db.Users.Where(u => u.Username.Equals(login)).Select(u => u.UserId)
                        .FirstOrDefault(),
                    PublishDate = DateTime.Now,
                    IsDeleted = false
                };
                db.Posts.Add(newPost);
                db.SaveChanges();
            }
        }

        public static void UpdatePost(int postId, string title, string content)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                var post = db.Posts.FirstOrDefault(p => p.PostId == postId);
                if (post != null)
                {
                    post.Title = title;
                    post.Content = content;
                    post.PublishDate = DateTime.Now;
                    
                    db.SaveChanges();
                }
            }
        }
    }
}