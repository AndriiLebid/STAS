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


        /// <summary>
        /// Create user
        /// </summary>
        /// <returns></returns>
        public async Task<User>? CreateUserAsync(User user)
        {
            List<Parm> parms = new()
            {
                new Parm("@UserId", SqlDbType.Int, null, 0, ParameterDirection.Output),
                new Parm("@UserName", SqlDbType.NVarChar, user.Name),
                new Parm("@Password", SqlDbType.NVarChar, user.Password),
                new Parm("@Salt", SqlDbType.Binary, user.Salt),
                new Parm("@UserTypeId", SqlDbType.Int, user.UserType)
            };

            if (await db.ExecuteNonQueryAsync("spAddUser", parms) > 0)
            {
                user.Id = (int?)parms.FirstOrDefault(p => p.Name == "@UserId")?.Value ?? 0;
            }
            else
            {
                throw new DataException("There was an error adding an user.");
            }

            return user;
        }

        /// <summary>
        /// Edit user
        /// </summary>
        /// <returns></returns>
        public async Task<User>? EditUserAsync(User user)
        {
            List<Parm> parms = new()
            {
                new Parm("@UserId", SqlDbType.Int, user.Id),
                new Parm("@UserName", SqlDbType.NVarChar, user.Name),
                new Parm("@UserTypeId", SqlDbType.Int, user.UserType)
            };

            if (await db.ExecuteNonQueryAsync("spEditUser", parms) > 0)
            {
                user.Id = (int?)parms.FirstOrDefault(p => p.Name == "@UserId")?.Value ?? 0;
            }
            else
            {
                throw new DataException("There was an error edited an user.");
            }

            return user;
        }

        /// <summary>
        /// Edit Password user
        /// </summary>
        /// <returns></returns>
        public async Task<User>? EditUserPassword(User user)
        {
            List<Parm> parms = new()
            {
                new Parm("@UserId", SqlDbType.Int, user.Id),
                new Parm("@Salt", SqlDbType.Binary, user.Salt),
                new Parm("@Password", SqlDbType.NVarChar, user.Password)
            };

            if (await db.ExecuteNonQueryAsync("spEditUserPassword", parms) > 0)
            {
                user.Id = (int?)parms.FirstOrDefault(p => p.Name == "@UserId")?.Value ?? 0;
            }
            else
            {
                throw new DataException("There was an error edited a password.");
            }

            return user;
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
