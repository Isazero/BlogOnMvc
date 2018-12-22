using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogModel;
using DevOne.Security.Cryptography.BCrypt;

namespace MvcBlogApi.Methods
{
    public class CheckMethods
    {
        public static bool CheckIsUserExist(string username, string password)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username.Equals(username));
                if (user != null && BCryptHelper.CheckPassword(password, user.Password))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsUserExists(string username)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                if (db.Users.Any(u => u.Username.Equals(username)))
                {
                    return true;
                }
            }
            return false;
        }
    }
}