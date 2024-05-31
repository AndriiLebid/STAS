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

        public Scan AddScanRecord(Scan scan)
        {
            List<Parm> parms = new()
            {
                new Parm("@ScanId", SqlDbType.Int, null, 0, ParameterDirection.Output),
                new Parm("@EmployeeId", SqlDbType.NVarChar, scan.EmployeeId),
                new Parm("@ScanTypeId", SqlDbType.Int, scan.ScanType),
                new Parm("@ScanDate", SqlDbType.DateTime2, DateTime.Now),
            };

            if (db.ExecuteNonQuery("spAddScan", parms) > 0)
            {
                scan.ScanId = (int?)parms.FirstOrDefault(p => p.Name == "@ScanId")?.Value ?? 0;
            }
            else
            {
                throw new DataException("There was an error adding a RowScan.");
            }

            return scan;
        }


        public async Task<Scan> AddScanRecordAsync(Scan scan)
        {
            List<Parm> parms = new()
            {
                new Parm("@ScanId", SqlDbType.Int, null, 0, ParameterDirection.Output),
                new Parm("@EmployeeId", SqlDbType.NVarChar, scan.EmployeeId),
                new Parm("@ScanTypeId", SqlDbType.Int, scan.ScanType),
                new Parm("@ScanDate", SqlDbType.DateTime2, DateTime.Now),
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

        public List<Scan> SearchScanByEmployeeId(int employeeId)
        {
            List<Parm> parms = new()
            {
                new Parm("@EmployeeId", SqlDbType.Int, employeeId),
            };

            DataTable dt = db.Execute("spSearchScanByEmployeeId", parms);

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




        public Scan GetLastScan(int employeeId)
        {
            List<Parm> parms = new()
            {
                new Parm("@EmployeeId", SqlDbType.Int, employeeId),
            };

            DataTable dt = db.Execute("spSearchLastScanByEmployeeId", parms);

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

        private Scan PopulateScan(DataRow dataRow)
        {
            Scan scan = new Scan();
            scan.ScanId = Convert.ToInt32(dataRow["RawScanId"]);
            scan.EmployeeId = Convert.ToInt32(dataRow["EmployeeId"]);
            scan.ScanType = Convert.ToInt32(dataRow["ScanType"]);
            scan.ScanDate = Convert.ToDateTime(dataRow["ScanDate"]); 

            return scan;
        }

        #endregion




    }
}
