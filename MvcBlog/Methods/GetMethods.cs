using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BlogModel;
using DevOne.Security.Cryptography.BCrypt;

namespace MvcBlog.Methods
{
    public class GetMethods
    {
        public static string GetLayout(string login)
        {
            if (CheckMethods.IsCurrentUserAdmin(login))
            {
                return "~/Views/Shared/_AdminIndexLayout.cshtml";
            }

            return "~/Views/Shared/_IndexLayout.cshtml";

        }

        public static string GetHash(string input)
        {
            // Use input string to calculate MD5 hash
            string salt = BCryptHelper.GenerateSalt();
            string hash = BCryptHelper.HashPassword(input, salt);
            return hash;
        }
        public static string GetSlug(string title)
        {
            string str = RemoveAccent(title).ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        //Метод нужен чтобы заранее определить User.Name
        //Если убрать метод то невозможно будет во View вытянуть имя юзера
        public static List<Post> GetInitializedUserList(List<Post> posts)
        {
            foreach (var p in posts)
            {
                var username = p.User.Name;
                var userSurname = p.User.Surname;
            }

            return posts;
        }
        public static List<Comment> GetInitializedUserList(List<Comment> comments)
        {
            foreach (var c in comments)
            {
                var username = c.User.Name;
                var userSurname = c.User.Surname;
            }

            return comments;
        }


        public static int GetUserId(string username)
        {
            using (BlogDbContext db = new BlogDbContext())
            {
                int userId = db.Users.Where(u => u.Username.Equals(username)).Select(u => u.UserId).FirstOrDefault();
                return userId;
            }
        }
    }
}