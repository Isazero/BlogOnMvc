using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogModel;

namespace MvcBlogApi.Methods
{
    public class InsertMethods
    {
        public static void RegisterNewUser(string username, string name, string surname, string password, string email)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                if (db.Users.Any(u => u.Username.Equals(username)))
                {
                    return;
                }
                User newUser = new User
                {
                    Username = username,
                    Password = Common.PasswordMethods.GetHash(password),
                    Name = name,
                    Surname = surname,
                    Email = email,
                    RoleId = 1,
                    IsDeleted = false
                };
                db.Users.Add(newUser);
                db.SaveChanges();
            }
        }
    }
}