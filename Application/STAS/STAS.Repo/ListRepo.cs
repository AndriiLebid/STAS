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
        /// Get Next and Previous records for scan table
        /// </summary>
        /// <returns></returns>
        public async Task<NCP> NCPScan(Scan sc)
        {

            List<Parm> parms = new()
            {
                new Parm("@ScanId", SqlDbType.Int, sc.ScanId),
                new Parm("@EmployeeId", SqlDbType.Int, sc.EmployeeId),
            };


            DataTable dt = await db.ExecuteAsync("GetLCNScanRows", parms);

            DataRow rw = dt.Rows[0];

            return new NCP
                   {
                       NextRecord = Convert.ToInt32(rw["NextValue"]),
                       CurrentRecord = Convert.ToInt32(rw["RawScanID"]),
                       PreviousRecord = Convert.ToInt32(rw["PreviousValue"]),
                   };

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
