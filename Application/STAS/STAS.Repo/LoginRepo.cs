using STAS.DAL;
using STAS.Model;
using STAS.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAS.Repo
{
    public class LoginRepo
    {
        private readonly DataAccess db = new();

        public byte[] GetUserSalt(string? userName)
        {
            List<Parm> parms = new()
            {
                new("@UserName", SqlDbType.NVarChar, userName, 30),
            };

            var salt  = (byte[])db.ExecuteScalar("spGetPasswordSalt", parms);

            return salt;

        }

        public async Task<LoginUser?> Login(LoginDTO user)
        {
            DataTable dt = await db.ExecuteAsync("spLogin",
                new List<Parm>
                {
                new Parm("@UserName", SqlDbType.NVarChar,user.UserName, 30),
                new Parm("@Password", SqlDbType.NVarChar, user.Password, 64)
                });

            if (dt.Rows.Count == 0)
                return null;

            DataRow row = dt.Rows[0];

            return new LoginUser
            {
                LoginUserId = Convert.ToInt32(row["UserId"]),
                LoginUserName = row["UserName"].ToString(),
                UserRole = row["UserRole"].ToString(),
            };
        }


    }
}
