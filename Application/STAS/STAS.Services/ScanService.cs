using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STAS.Model;
using STAS.Repo;
using STAS.Type;

namespace STAS.Services
{
    public class ScanService
    {

        #region Private Fields

            private readonly ScanRepo repo = new();

        #endregion

        #region Public Methods
        public Scan AddScanRecord(Scan scan)
        {
            if (ValidationScan(scan))
            {
                return repo.AddScanRecord(scan);
            }
            else
            {
                throw new Exception("RawScan Validation error happend");
            }
 
        }

        public async Task<Scan> AddScanRecordAsynk(Scan scan)
        {
            if (ValidationScan(scan))
            {
                return await repo.AddScanRecordAsync(scan);
            }
            else
            {
                throw new Exception("RawScan Validation error happend");
            }

        }



        public List<Scan> SearchScanByEmployeeId(int employeeId)
        {
            return repo.SearchScanByEmployeeId(employeeId);
        }

        public List<Scan> SearchScanByDate(DateTime? startDate, DateTime? endDate)
        {
            return repo.SearchScanByDate(startDate, endDate);
        }
        

        public Scan SearchLastScanByEmployeeId(int employeeId)
        {
            return repo.GetLastScan(employeeId);
        }

        public async Task<Scan> SearchLastScanByEmployeeIdAsync(int employeeId)
        {
            return await repo.GetLastScanAsync(employeeId);
        }


        #endregion



        #region Private Methods

        private bool ValidationScan(Scan scan)
        {
            Scan lastScan = repo.GetLastScan(scan.EmployeeId);
            if (lastScan == null) return true;

            if (lastScan.ScanDate < scan.ScanDate && lastScan.ScanType != scan.ScanType) return true;
            
            return false;
        }

        #endregion

    }
}
