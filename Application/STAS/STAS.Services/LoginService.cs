using STAS.Model;
using STAS.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAS.Services
{
    public class LoginService
    {
        #region Fields

        private readonly LoginRepo repo = new();

        #endregion

        #region  Public Methods

        /// <summary>
        /// Get user salt
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<LoginUser?> Login(LoginDTO user)
        {
            byte[] salt = repo.GetUserSalt(user.UserName);

            if (salt == null)
                return null;

            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
                return null;

            user.Password = PasswordUtility.HashToSHA256(user.Password, salt);


            return await repo.Login(user);

        }

        #endregion



    }
}
