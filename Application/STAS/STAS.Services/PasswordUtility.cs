using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace STAS.Services
{
    public class PasswordUtility
    {
        public static byte[] GenerateSalt(int length = 16)
        {
            byte[] salt = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;

        }

        public static string HashToSHA256(string input, byte[] salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] saltedPassword = salt.Concat(Encoding.UTF8.GetBytes(input)).ToArray();
                byte[] hashBytes = sha256.ComputeHash(saltedPassword);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }


        }

        public static bool PasswordCheck(string password)
        {
            string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[^\w\d\s]).{6,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(password);
        }
    }
}
