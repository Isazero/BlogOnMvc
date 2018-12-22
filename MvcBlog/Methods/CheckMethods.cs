using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogModel;
using DevOne.Security.Cryptography.BCrypt;

namespace MvcBlog.Methods
{
    public class CheckMethods
    {
        public static bool Authenticate(string username, string password)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                var entity = db.Users.FirstOrDefault(u => u.Username == username);

                if (entity != null && BCryptHelper.CheckPassword(password, entity.Password))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsCurrentUserAdmin(string username)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                List<User> users = db.Users.Where(u => u.RoleId == 2).ToList();
                foreach (var u in users)
                {
                    if (u.Username == username)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsUserExists(string login)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                if (db.Users.Any(u=>u.Username.Equals(login)))
                {
                    return true;
                }
                
            }
            return false;
        }

        public static bool IsUsersComment(int userId, int commentId)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                if (db.Comments.Any(c => c.CommentId == commentId && c.UserId==userId))
                {
                    return true;
                }

            }
            return false;
        }

        public static bool IsUsersPost(int postid, string username)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                int userId = GetMethods.GetUserId(username);
                if (db.Posts.Any(p => p.PostId == postid && p.UserId == userId))
                {
                    return true;
                }

            }
            return false;
        }
    }
}