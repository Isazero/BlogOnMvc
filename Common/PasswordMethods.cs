using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevOne.Security.Cryptography.BCrypt;

namespace Common
{
    public class PasswordMethods
    {
        public static string GetHash(string input)
        {
            // Use input string to calculate MD5 hash
            string salt = BCryptHelper.GenerateSalt();
            string hash = BCryptHelper.HashPassword(input, salt);
            return hash;
        }
    }
}
