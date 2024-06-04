using STAS.Model;
using STAS.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAS.Services
{
    public class ListService
    {
        #region Private Fields

        private readonly ListRepo repo = new();

        #endregion

        #region Public Methods

        /// <summary>
        /// Get all scan type
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScanType>> GetTypeScan()
        {
            return await repo.GetTypeScan();
        }

        /// <summary>
        /// Get next, current and previous records from scan table.
        /// </summary>
        /// <param name="scan"></param>
        /// <returns></returns>

        public async Task<NCP> NCPScan(Scan scan)
        {
            return await repo.NCPScan(scan);
        }


        /// <summary>
        /// Get Employee Id By Card Number
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public async Task<int> GetEmployeeIdByNumber(string cardNumber)
        {
            return await repo.GetEmployeeIdByNumber(cardNumber);
        }

        /// <summary>
        /// Get Employee List
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public async Task<List<Employee>> GetAllEmployee()
        {
            return await repo.GetAllEmployee();
        }

        #endregion
    }
}
