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

        /// <summary>
        /// Generate Salt for user
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] GenerateSalt(int length = 16)
        {
            byte[] salt = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;

        }

        /// <summary>
        /// Hash user password
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string HashToSHA256(string input, byte[] salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] saltedPassword = salt.Concat(Encoding.UTF8.GetBytes(input)).ToArray();
                byte[] hashBytes = sha256.ComputeHash(saltedPassword);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }


        }


        /// <summary>
        /// Validate password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool PasswordCheck(string password)
        {
            string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[^\w\d\s]).{6,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(password);
        }
    }
}
