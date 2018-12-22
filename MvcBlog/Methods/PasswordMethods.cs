using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using DevOne.Security.Cryptography.BCrypt;

namespace MvcBlog.Methods
{
    public class PasswordMethods
    {
        public static string CreateHash(string input)
        {
            // Use input string to calculate MD5 hash
            string salt = BCryptHelper.GenerateSalt();
            string hash = BCryptHelper.HashPassword(input, salt);
            return hash;
        }
    }
}