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
        public async Task<List<ScanType>> GetTypeScan()
        {
            return await repo.GetTypeScan();
        }

        public async Task<int> GetEmployeeIdByNumber(string cardNumber)
        {
            return await repo.GetEmployeeIdByNumber(cardNumber);
        }

        #endregion
    }
}
