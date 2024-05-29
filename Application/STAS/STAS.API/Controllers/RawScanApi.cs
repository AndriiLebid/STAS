using Microsoft.AspNetCore.Mvc;
using STAS.Services;
using STAS.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STAS.API.Controllers
{
    [Route("api/scan")]
    [ApiController]
    public class RawScanApi : ControllerBase
    {
        private readonly ScanService service = new();
        private readonly ListService list = new();


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Scan>> SetNewRecord(ScanApiDTO scanIn)
        {
            try
            {

                if (scanIn == null)
                {
                    return BadRequest("The RawScan is not specified.");
                }

                List<ScanType> scanTypeList = await list.GetTypeScan();
                scanTypeList = scanTypeList.Where(d => d.TypeName == scanIn.ScanType).ToList();

                if (scanTypeList.Count == 0)
                {
                    return NotFound(new List<Scan>());
                }

                int? empId = await list.GetEmployeeIdByNumber(scanIn.EmployeeCardNumber);

                if (empId == null)
                {
                    return NotFound(new List<Scan>());
                }

                Scan scan = new Scan();
                scan.ScanDate = scanIn.ScanDate;
                scan.ScanType = scanTypeList[0].TypeId;
                scan.EmployeeId = (int)empId;

                var scanOut = service.AddScanRecord(scan);
                return Ok(scanOut);
            }
            catch (Exception)
            {
                return Problem(title: "An internal error has occurred. Please contact the system administrator");

            }
        }

    }
}
