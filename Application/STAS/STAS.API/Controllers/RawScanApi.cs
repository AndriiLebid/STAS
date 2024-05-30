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
        private readonly EmployeeService employee = new();


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
                scanTypeList = scanTypeList.Where(d => d.TypeName == scanIn.ScanType.ToUpper()).ToList();

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


                Scan scanOut = new Scan();

                try
                {
                    scanOut = await service.AddScanRecordAsynk(scan);
                }
                catch (Exception ex) { 

                    return BadRequest(ex.Message);

                }
                
                return Ok(scanOut);
            }
            catch (Exception)
            {
                return Problem(title: "An internal error has occurred. Please contact the system administrator");

            }
        }


        [HttpGet("getLastScanByEmployeeId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Scan>> GetLastScan(string? employeeId = null)
        {
            try
            {

                if (String.IsNullOrEmpty(employeeId))
                {
                    return BadRequest("The Employee is not specified.");
                }


                if (!int.TryParse(employeeId, out int id))
                {
                    return BadRequest("The Employee ID is incorrect; please enter an integer.");
                }

                Employee emp = await employee.SearchEmployeeByIdAsync(id);

                if (emp.EmployeeId != id)
                {
                    return BadRequest("The employee is not in the database.");
                }

                Scan scans = await service.SearchLastScanByEmployeeIdAsync(id);

                if (scans == null)
                {
                    return NotFound(new Scan());
                }

                return Ok(scans);
            }
            catch (Exception)
            {
                return Problem(title: "An internal error has occurred. Please contact the system administrator");

            }
        }

    }
}
