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
    public class UserRepo
    {

        #region Private Fields

        private readonly DataAccess db = new();

        #endregion

        #region Public Fields
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAllUserAsync()
        {

            DataTable dt = await db.ExecuteAsync("spGetAllUser");

            List<User> usr = new List<User>();

            if (dt.Rows.Count != 0)
            {
                return dt.AsEnumerable().Select(row => PopulateUser(row)).ToList();
            }
            else
            {
                return usr;
            }
        }


        /// <summary>
        /// Get user by id
        /// </summary>
        /// <returns></returns>
        public async Task<User>? GetUserByIdAsync(int id)
        {
            List<Parm> parms = new()
            {
                new Parm("@UserId", SqlDbType.Int, id),
            };

            DataTable dt = await db.ExecuteAsync("spGetUserById", parms);

            List<User> usr = new List<User>();

            if (dt.Rows.Count != 0)
            {
                return PopulateUser(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }


        public async Task<int> DeleteUserAsync(int id)
        {
            List<Parm> parms = new()
            {
                new Parm("@UserId", SqlDbType.Int, id),
            };

           return await db.ExecuteNonQueryAsync("spDeleteUser", parms);

        }

        #endregion

        #region Private Methods

        private User PopulateUser(DataRow row)
        {
            User user = new User();
            user.Id = Convert.ToInt32(row["UserId"]);
            user.Name = row["UserName"].ToString();
            return user;
        }


        #endregion



    }
}
