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
            return repo.AddScanRecord(scan);
        }

        public List<Scan> SearchScanByEmployeeId(int employeeId)
        {
            return repo.SearchScanByEmployeeId(employeeId);
        }

        public List<Scan> SearchScanByDate(DateTime? startDate, DateTime? endDate)
        {
            return repo.SearchScanByDate(startDate, endDate);
        }


        #endregion



        #region Private Methods


        #endregion

    }
}
