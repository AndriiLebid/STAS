using STAS.DAL;
using STAS.Model;
using STAS.Type;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAS.Repo
{
    public class ScanRepo
    {
        #region Private Fields

        private readonly DataAccess db = new();

        #endregion

        #region Public Methods

       /// <summary>
       /// Add new scan
       /// </summary>
       /// <param name="scan"></param>
       /// <returns></returns>
       /// <exception cref="DataException"></exception>
        public async Task<Scan> AddScanRecordAsync(Scan scan)
        {
            List<Parm> parms = new()
            {
                new Parm("@ScanId", SqlDbType.Int, null, 0, ParameterDirection.Output),
                new Parm("@EmployeeId", SqlDbType.NVarChar, scan.EmployeeId),
                new Parm("@ScanTypeId", SqlDbType.Int, scan.ScanType),
                new Parm("@ScanDate", SqlDbType.DateTime2, scan.ScanDate),
            };

            if (await db.ExecuteNonQueryAsync("spAddScan", parms) > 0)
            {
                scan.ScanId = (int?)parms.FirstOrDefault(p => p.Name == "@ScanId")?.Value ?? 0;
            }
            else
            {
                throw new DataException("There was an error adding a RowScan.");
            }

            return scan;
        }

        /// <summary>
        /// Add new scan
        /// </summary>
        /// <param name="scan"></param>
        /// <returns></returns>
        /// <exception cref="DataException"></exception>
        public async Task<Scan> UpdateRecordAsync(Scan scan)
        {
            List<Parm> parms = new()
            {
                new Parm("@ScanId", SqlDbType.Int, scan.ScanId),
                new Parm("@EmployeeId", SqlDbType.NVarChar, scan.EmployeeId),
                new Parm("@ScanTypeId", SqlDbType.Int, scan.ScanType),
                new Parm("@ScanDate", SqlDbType.DateTime2, scan.ScanDate),
                new Parm("@RecordVersion", SqlDbType.Timestamp, scan.RecordVersion)
            };

            if (await db.ExecuteNonQueryAsync("spUpdateScan", parms) > 0)
            {
                scan.ScanId = (int?)parms.FirstOrDefault(p => p.Name == "@ScanId")?.Value ?? 0;
            }
            else
            {
                throw new DataException("There was an error adding a RowScan.");
            }

            return scan;
        }





        /// <summary>
        /// Get Scan By Employee Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        //public List<Scan> SearchScanByEmployeeId(int employeeId)
        //{
        //    List<Parm> parms = new()
        //    {
        //        new Parm("@EmployeeId", SqlDbType.Int, employeeId),
        //    };

        //    DataTable dt = db.Execute("spSearchScanByEmployeeId", parms);

        //    List<Scan> scans = new List<Scan>();

        //    if (dt != null)
        //    {
        //        return dt.AsEnumerable().Select(row => PopulateScan(row)).ToList();
        //    }
        //    else
        //    {
        //        return scans;
        //    }
        //}


        /// <summary>
        /// Search Scan By Employee Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<List<Scan>> SearchScanByEmployeeIdAsync(int employeeId)
        {
            List<Parm> parms = new()
            {
                new Parm("@EmployeeId", SqlDbType.Int, employeeId),
            };

            DataTable dt = await db.ExecuteAsync("spSearchScanByEmployeeId", parms);

            List<Scan> scans = new List<Scan>();

            if (dt != null)
            {
                return dt.AsEnumerable().Select(row => PopulateScan(row)).ToList();
            }
            else
            {
                return scans;
            }
        }



        /// <summary>
        /// Get last scan
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public Scan? GetLastScan(int employeeId)
        {
            List<Parm> parms = new()
            {
                new Parm("@EmployeeId", SqlDbType.Int, employeeId),
            };

            DataTable dt = db.Execute("spSearchLastScanByEmployeeId", parms);

            Scan scans = new Scan();

            if (dt.Rows.Count != 0)
            {
                return PopulateScan(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public async Task<Scan> GetLastScanAsync(int employeeId)
        {
            List<Parm> parms = new()
            {
                new Parm("@EmployeeId", SqlDbType.Int, employeeId),
            };

            DataTable dt = await db.ExecuteAsync("spSearchLastScanByEmployeeId", parms);

            Scan scans = new Scan();

            if (dt != null)
            {
                return PopulateScan(dt.Rows[0]);
            }
            else
            {
                return scans;
            }
        }


        public async Task<Scan> GetScanByIdAsync(int scanId)
        {
            List<Parm> parms = new()
            {
                new Parm("@ScanId", SqlDbType.Int, scanId),
            };

            DataTable dt = await db.ExecuteAsync("spGetScanById", parms);

            Scan scans = new Scan();

            if (dt != null)
            {
                return PopulateScan(dt.Rows[0]);
            }
            else
            {
                return scans;
            }
        }


        /// <summary>
        /// Search Scan By Date
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<Scan> SearchScanByDate(DateTime? startDate, DateTime? endDate)
        {
            List<Parm> parms = new()
            {
                new Parm("@StartDate", SqlDbType.DateTime, startDate),
                new Parm("@EndDate", SqlDbType.DateTime, endDate),
            };

            DataTable dt = db.Execute("spSearchScanByDate", parms);

            List<Scan> scans = new List<Scan>();

            if (dt != null)
            {
                return dt.AsEnumerable().Select(row => PopulateScan(row)).ToList();
            }
            else
            {
                return scans;
            }

        }




        #endregion


        #region Private Methods

        /// <summary>
        /// Populate Scan Object
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private Scan PopulateScan(DataRow dataRow)
        {
            Scan scan = new Scan();
            scan.ScanId = Convert.ToInt32(dataRow["RawScanId"]);
            scan.EmployeeId = Convert.ToInt32(dataRow["EmployeeId"]);
            scan.ScanType = Convert.ToInt32(dataRow["ScanType"]);
            scan.ScanDate = Convert.ToDateTime(dataRow["ScanDate"]);
            scan.RecordVersion = (byte[])dataRow["RecordVersion"];

            return scan;
        }

        #endregion




    }
}
