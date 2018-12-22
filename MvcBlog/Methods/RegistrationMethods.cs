using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogModel;
using MvcBlog.Models;

namespace MvcBlog.Methods
{
    public class RegistrationMethods
    {
        public static bool RegisterNewUser(RegisterModel model)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                if (db.Users.Any(u => u.Username.Equals(model.Login)))
                {
                    return false;
                }
                User newUser = new User
                {
                    Username = model.Login,
                    Password = PasswordMethods.CreateHash(model.Password),
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    RoleId = 1,
                    IsDeleted = false
                };
                db.Users.Add(newUser);
                db.SaveChanges();

                if (db.Users.Any(u => u.Username.Equals(newUser.Username)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}