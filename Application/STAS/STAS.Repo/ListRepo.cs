using Microsoft.IdentityModel.Tokens;
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
    public class ListRepo
    {

        #region Private Fields

        private readonly DataAccess db = new();

        #endregion

        #region Public Methods

        /// <summary>
        /// Get All Type Scans
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScanType>> GetTypeScan()
        {
            DataTable dt = db.Execute("spGetTypeScan");

            return dt.AsEnumerable().Select(row =>
                new ScanType
                {
                    TypeId = Convert.ToInt32(row["TypeId"]),
                    TypeName = row["TypeName"].ToString()
                }).ToList();

        }

        /// <summary>
        /// Get All User Type
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserType>> GetUserType()
        {
            DataTable dt = await db.ExecuteAsync("spGetRole");

            return dt.AsEnumerable().Select(row =>
                new UserType
                {
                    TypeId = Convert.ToInt32(row["RoleId"]),
                    TypeName = row["RoleName"].ToString()
                }).ToList();

        }

        /// <summary>
        /// Get All Employee
        /// </summary>
        /// <returns></returns>
        public async Task<List<Employee>> GetAllEmployee()
        {
            DataTable dt = await db.ExecuteAsync("spGetAllEmployees");

            return dt.AsEnumerable().Select(row =>
                new Employee
                {
                    EmployeeId = Convert.ToInt32(row["EmployeeId"]),
                    FirstName = row["FirstName"].ToString(),
                    MiddleInitial = row["MiddleInitial"].ToString(),
                    LastName = row["LastName"].ToString(),
                    TypeEmployeeId = Convert.ToInt32(row["TypeEmployeeId"]),
                    EmployeeNumber = row["EmployeeCardNumber"].ToString(),
                    RecordVersion = (byte[])row["RecordVersion"]
                }).ToList();

        }

        /// <summary>
        /// Get next, current and previous records from scan table.
        /// </summary>
        /// <returns></returns>
        public async Task<NCP> NCPScan(Scan sc)
        {

            List<Parm> parms = new()
            {
                new Parm("@ScanId", SqlDbType.Int, sc.ScanId),
                new Parm("@EmployeeId", SqlDbType.Int, sc.EmployeeId),
            };


            DataTable dt = await db.ExecuteAsync("spGetLCNScanRows", parms);

            DataRow rw = dt.Rows[0];

            NCP ncp = new();

            ncp.NextRecord = int.TryParse(rw["NextValue"].ToString(), out int nextValue) ? nextValue : null;
            ncp.CurrentRecord = int.TryParse(rw["RawScanID"].ToString(), out int currentValue) ? currentValue : null;
            ncp.PreviousRecord = int.TryParse(rw["PreviousValue"].ToString(), out int PreviousValue) ? PreviousValue : null;

           return ncp;

        }


        /// <summary>
        /// Get Employee Id By Number
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<int> GetEmployeeIdByNumber(string cardNumber)
        {
            List<Parm> parms = new()
                {
                    new Parm("@CardNumber", SqlDbType.NVarChar, cardNumber, 4),
                };

            var EmployeeId = await db.ExecuteScalarAsync("spGetEmployeeIdByCardNumber", parms);

            if (EmployeeId != null)
            {
                return Convert.ToInt32(EmployeeId);
            }
            else
            {
                throw new InvalidOperationException("No changes were made to the Employee record. This may be due to no changes in data or concurrent updates.");
            }

        }


        #endregion

    }
}
