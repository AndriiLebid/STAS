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
            private readonly ListService list = new();

        #endregion

        #region Public Methods

        /// <summary>
        /// Add new scan record
        /// </summary>
        /// <param name="scan"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Scan> AddScanRecordAsync(Scan scan)
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


        /// <summary>
        /// Update scan record
        /// </summary>
        /// <param name="scan"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Scan> UpdateRecordAsync(Scan scan)
        {
            if (await ValidationScanUpdate(scan))
            {
                return await repo.UpdateRecordAsync(scan);
            }
            else
            {
                throw new Exception("RawScan Validation error happend");
            }

        }





        /// <summary>
        /// Get Scan By Employee Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<List<Scan>> SearchScanByEmployeeIdAsync(int employeeId)
        {
            return await repo.SearchScanByEmployeeIdAsync(employeeId);
        }

        public List<Scan> SearchScanByDate(DateTime? startDate, DateTime? endDate)
        {
            return repo.SearchScanByDate(startDate, endDate);
        }
        

        public Scan SearchLastScanByEmployeeId(int employeeId)
        {
            return repo.GetLastScan(employeeId);
        }

        /// <summary>
        /// Search Last Sca nBy Employee Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<Scan> SearchLastScanByEmployeeIdAsync(int employeeId)
        {
            return await repo.GetLastScanAsync(employeeId);
        }

        public async Task<Scan> GetScanByIdAsync(int scanId)
        {
            return await repo.GetScanByIdAsync(scanId);
        }


        #endregion



        #region Private Methods
        /// <summary>
        /// Verification RawScan
        /// </summary>
        /// <param name="scan"></param>
        /// <returns></returns>
        private bool ValidationScan(Scan scan)
        {
            Scan lastScan = repo.GetLastScan(scan.EmployeeId);
            if (lastScan == null) return true;

            if (lastScan.ScanDate <= scan.ScanDate && lastScan.ScanType != scan.ScanType) return true;
            
            return false;
        }


        private async Task<bool> ValidationScanUpdate(Scan scan)
        {
            NCP ncp = await list.NCPScan(scan);

           
            Scan previousScan = await repo.GetScanByIdAsync(ncp.PreviousRecord ?? -1);
            Scan nextScan = await repo.GetScanByIdAsync(ncp.NextRecord ?? -1);


         
            if (previousScan.ScanDate <= scan.ScanDate && nextScan.ScanDate > scan.ScanDate) return true;

            return false;
        }
        #endregion

    }
}
