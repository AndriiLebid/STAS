using Microsoft.AspNetCore.Mvc;
using STAS.Services;
using STAS.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;

namespace STAS.API.Controllers
{
    [Route("api/scan")]
    [ApiController]
    public class RawScanApi : ControllerBase
    {
        #region Fields

        private readonly ScanService service = new();
        private readonly ListService list = new();
        private readonly EmployeeService employee = new();

        #endregion

        #region  Public Methods

        /// <summary>
        /// Endpoint for add new scan record
        /// </summary>
        /// <param name="scanIn"></param>
        /// <returns></returns>

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
                    return BadRequest("The RawScan is not specified in correct way.");
                }

                List<ScanType> scanTypeList = await list.GetTypeScan();
                scanTypeList = scanTypeList.Where(d => d.TypeName.Equals(scanIn.ScanType.ToUpper())).ToList();

                if (scanTypeList.Count == 0)
                {
                    return NotFound("Error Scan Type");
                }

                int? empId = await list.GetEmployeeIdByNumber(scanIn.EmployeeCardNumber);

                if (empId == null)
                {
                    return NotFound("Wrong Employee");
                }

                Scan scan = new();
                scan.ScanDate = scanIn.ScanDate;
                scan.ScanType = scanTypeList[0].TypeId;
                scan.EmployeeId = (int)empId;


                Scan scanOut = new();

                try
                {
                    scanOut = await service.AddScanRecordAsync(scan);
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

        /// <summary>
        /// Add list scan records to DB
        /// </summary>
        /// <param name="scanIn"></param>
        /// <returns></returns>
        [HttpPost("addListOfScans")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Scan>>> SyncAllNewRecords(List<ScanApiDTO> scanInList)
        {
            try
            {
                List<Scan> scanOutList = new();
                List<ScanType> scanTypeList = await list.GetTypeScan();
                if (IsList(scanInList))
                {
                    return BadRequest("The RawScan is not specified or not a list.");
                }

                foreach (var sc in scanInList)
                {
                    var scanType = scanTypeList.FirstOrDefault(d => d.TypeName.Equals(sc.ScanType.ToUpper()));
                    if (scanType == null)
                    {
                        return NotFound("Wrong Scan Type");
                    }

                    int? empId = await list.GetEmployeeIdByNumber(sc.EmployeeCardNumber);

                    if (empId == null)
                    {
                        return NotFound("Wrong Employee");
                    }

                    var scan = new Scan
                    {
                        ScanDate = sc.ScanDate,
                        ScanType = scanType.TypeId,
                        EmployeeId = (int)empId
                    };

                    scan.ScanDate = sc.ScanDate;
                    scan.ScanType = scanType.TypeId;
                    scan.EmployeeId = (int)empId;

                    try
                    {
                        var scanOut = await service.AddScanRecordAsync(scan);
                        scanOutList.Add(scanOut);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }

                }

                return Ok(scanOutList);
            }
            catch (Exception)
            {
                return Problem(title: "An internal error has occurred. Please contact the system administrator");

            }
        }


        /// <summary>
        /// Get Last Scan record By EmployeeId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>

        [HttpGet("getLastScanByEmployeeId/{employeeId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Scan>> GetLastScan(string? employeeId)
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

                Scan scan = await service.SearchLastScanByEmployeeIdAsync(id);

                if (scan == null)
                {
                    return NotFound(new Scan());
                }

                return Ok(scan);
            }
            catch (Exception)
            {
                return Problem(title: "An internal error has occurred. Please contact the system administrator");

            }
        }



        /// <summary>
        /// Check connection
        /// </summary>
        /// <param></param>
        /// <returns></returns>

        [HttpGet("check")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckServer()
        {
            try
            {
                var scan = true; 
                if (scan)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        #endregion


        #region  Private Methods
        /// <summary>
        /// Check parameter is a list
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private bool IsList(object list)
        {
            if (list == null) return false;
            return list is IList &&
                   list.GetType().IsGenericType &&
                   list.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }
        #endregion

    }
}
