using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STAS.Services;
using STAS.Model;
using STAS.Web.Models;
using PagedList;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using static NuGet.Packaging.PackagingConstants;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.IdentityModel.Tokens;

namespace STAS.Web.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly EmployeeService service = new();
        private readonly ScanService scanService = new();
        private readonly ListService listService = new();

        // GET: EmployeeController
        public async Task<ActionResult> Index(int? page)
        {

            //check login
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return RedirectToAction("Login", "Login");
            }

            // Create pegination consts
            const int pageSize = 8;
            int pageNumber = (page ?? 1);

            try
            {
                List<Employee> employees = await service.GetAllEmployeesAsync();
                if (employees.Count == 0)
                {
                    TempData["EmployeeEmpty"] = "You don't have employees yet.";
                }
            
                var pagedEmp = employees.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                ViewBag.TotalPages = (int)Math.Ceiling((double)employees.Count / pageSize);
                ViewBag.CurrentPage = pageNumber;

                return View(pagedEmp);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
            
        }

        // GET: EmployeeController Search method
        public async Task<ActionResult> Search(string? startDate, string? endDate, int? employeeId, int? status)
        {

            //check login
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return RedirectToAction("Login", "Login");
            }

            // create classes and list of employees
            EmployeeWithScans employeeWithScans = new EmployeeWithScans();
            employeeWithScans.EmployeesList = await GetEmployeeList();
            employeeWithScans.ScanTypesList = await GetTypes();
            List<ScanVM> scansVm = new List<ScanVM>();

            try
            {
                
                if (employeeId == null)
                { 
                    return View(employeeWithScans);
                }

                ViewBag.StartDate = startDate;
                ViewBag.EndDate = endDate;
                ViewBag.Status = status;
                ViewBag.EmployeeId = employeeId;

                List<ScanType> types = await listService.GetTypeScan();
                Employee employee = await service.SearchEmployeeByIdAsync((int)employeeId!);
                List<Scan> scans = await scanService.SearchScanByEmployeeIdAsync((int)employeeId);

                // search by date
                if(!String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
                {
                    scans = scans.Where(sc => sc.ScanDate >= Convert.ToDateTime(startDate)
                          && sc.ScanDate <= Convert.ToDateTime(endDate)).ToList();
                }
                else if (String.IsNullOrEmpty(startDate) && !String.IsNullOrEmpty(endDate))
                {
                    scans = scans.Where(sc => sc.ScanDate <= Convert.ToDateTime(endDate)).ToList();
                }
                else if (!String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate))
                {
                    scans = scans.Where(sc => sc.ScanDate >= Convert.ToDateTime(startDate)).ToList();
                }

                //Filter by type
                if (status != null)
                {
                    scans = scans.Where(sc => sc.ScanType.Equals(status)).ToList();
                }

                foreach (var sc in scans)
                {
                    List<ScanType> type = types;
                    ScanVM scanVM = new ScanVM()
                    {
                        ScanId = sc.ScanId,
                        ScanDate = sc.ScanDate,
                        ScanType = type.FirstOrDefault(t => t.TypeId == sc.ScanType).TypeName
                    };

                    scansVm.Add(scanVM);
                }

                employeeWithScans.Scans = scansVm;
                employeeWithScans.Employee = employee;

                return View(employeeWithScans);
        }
            catch (Exception ex)
            {
                return ShowError(ex);
            }

        }

        // GET: EmployeeController Calculated Shifts method
        public async Task<ActionResult> Shift(string? startDate, int? employeeId)
        {

            //check login
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return RedirectToAction("Login", "Login"); 
            }

            try
            {
                ViewBag.StartDate = startDate;
                ViewBag.EmployeeId = employeeId;


                ShiftVM shiftVMobject = new ShiftVM()
                {
                    EmployeesList = await GetEmployeeList(),
                    Shifts = new List<Shift>()
                };

                // Check entered data
                if (startDate == null)
                {
                    return View(shiftVMobject);
                }

                if (employeeId == null)
                {
                    return View(shiftVMobject);
                }

                //get employee and scans records
                shiftVMobject.Employee = await service.SearchEmployeeByIdAsync((int)employeeId!);
                List<Scan> scans = await scanService.SearchScanByEmployeeIdAsync((int)employeeId);

                //Calculate end date
                DateTime startDateTime = Convert.ToDateTime(startDate);
                DateTime endDateTime = startDateTime.AddDays(14);

                // filter scans by date and order it
                scans = scans.Where(sc => sc.ScanDate >= startDateTime
                          && sc.ScanDate <= endDateTime).ToList();
                scans = scans.OrderBy(s => s.ScanDate).ToList();

                //Ctera shift instance
                Shift currentShift = new();

                // Populate shifts

                if (scans.Count > 0) {

                    foreach (var sc in scans)
                    {
                        if (sc.ScanType == 1)
                        {
                            currentShift = new Shift
                            {
                                StartDate = sc.ScanDate,
                                EmployeeName = shiftVMobject.Employee.FullName
                            };
                        }
                        else if (sc.ScanType == 2)
                        {
                            if (currentShift == null)
                            {
                                currentShift = new Shift
                                {
                                    StartDate = startDateTime,
                                    EndDate = sc.ScanDate,
                                    EmployeeName = shiftVMobject.Employee.FullName
                                };
                                shiftVMobject.Shifts.Add(currentShift);
                                currentShift = null;
                            }
                            else
                            {
                                currentShift.EndDate = sc.ScanDate;
                                shiftVMobject.Shifts.Add(currentShift);
                                currentShift = null;
                            }
                        }
                        else if (sc.ScanType == 3)
                        {
                            currentShift.StartBreak = sc.ScanDate;
                        }
                        else if (sc.ScanType == 4)
                        {
                            currentShift.EndBreak = sc.ScanDate;
                        }
                        else if (sc.ScanType == 5)
                        {
                            currentShift.StartLunch = sc.ScanDate;
                        }
                        else if (sc.ScanType == 6)
                        {
                            currentShift.EndLunch = sc.ScanDate;
                        }
                    }

                    if (currentShift != null)
                    {
                        currentShift.EndDate = currentShift.StartDate.Date.AddDays(1).AddTicks(-1);
                        shiftVMobject.Shifts.Add(currentShift);
                    }
                }

                if (shiftVMobject.Shifts.Count !=0)
                {
                    foreach (Shift shift in shiftVMobject.Shifts)
                    {
                        shiftVMobject.TotalDuration += shift.Duration;
                    }
                }

                var Result = shiftVMobject.TotalDuration.TotalHours;

                return View(shiftVMobject);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }

        }

        // GET: EmployeeController/ShiftDetails
        public async Task<ActionResult> ShiftDetails(int? id, DateTime start, DateTime end)
        {
            try
            {

                if (id == null || start == DateTime.MinValue || end == DateTime.MinValue)
                {
                    return new BadRequestResult();
                }

                Shift shift = await service.GetShiftAsync((int)id, start, end);

                return View(shift);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
        }

        // GET: EmployeeController/Details/5
        public async Task<ActionResult> Details(int? id)
        {

            //check login
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return RedirectToAction("Login", "Login");
            }


            try
            {

                EmployeeWithScans employeeWithScans = new EmployeeWithScans();
                List<ScanVM> scansVm = new List<ScanVM>();

                if (id == null)
                    return new BadRequestResult();

                Employee employee = await service.SearchEmployeeByIdAsync((int)id);
                if (employee == null)
                {
                    return new NotFoundResult();
                }
                List<ScanType> types = await listService.GetTypeScan();
                List <Scan> scans = await scanService.SearchScanByEmployeeIdAsync((int)id);

                foreach (var sc in scans)
                {
                    List<ScanType> type = types;
                    ScanVM scanVM = new ScanVM()
                    {
                        ScanId = sc.ScanId,
                        ScanDate = sc.ScanDate,
                        ScanType = type.FirstOrDefault(t => t.TypeId == sc.ScanType).TypeName
                    };

                    scansVm.Add(scanVM);
                }

                employeeWithScans.Scans = scansVm;
                employeeWithScans.Employee = employee;

                return View(employeeWithScans);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {

            //check login
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return RedirectToAction("Login", "Login");
            }


            try
            {

                Employee employee = new Employee();
                employee.TypeEmployeeId = 2;

                return View(employee);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }

        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(employee);
                }

                var result = await service.AddEmployeeAsync(employee);

                if (result.Errors.Count != 0)
                {
                    string errorMessage = "";
                    foreach (var error in result.Errors)
                    {
                        errorMessage += error.Description + " ";
                    }
                    ViewBag.ErrorMessage = errorMessage;
                    return View(employee);
                }

                TempData["Success"] = "Employee Created successfully.";
                return RedirectToAction("Index", "Employee");

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction("Index", "Employee");
            }
        }

        // GET: EmployeeController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                Employee employee = await service.SearchEmployeeByIdAsync(id);
                return View(employee);
            }
            catch (Exception ex)
            {
                return ShowError(ex);
            }
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(employee);
                }

                var result = await service.UpdateEmployeeAsync(employee);

                if (result.Errors.Count != 0)
                {
                    string errorMessage = "";
                    foreach (var error in result.Errors)
                    {
                        errorMessage += error.Description + " ";
                    }
                    ViewBag.ErrorMessage = errorMessage;
                    return View(employee);
                }
                TempData["Success"] = "Employee Updated successfully.";
                return RedirectToAction("Index", "Employee");

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction("Index", "Employee");
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Inactive(int id)
        {
            try
            {
                Employee employee = await service.SearchEmployeeByIdAsync(id);

                if (employee == null)
                {
                    TempData["Error"] = "Employee not found.";
                    return RedirectToAction("Index", "Employee");
                }

                employee.TypeEmployeeId = 1;

                var result = await service.UpdateEmployeeAsync(employee);

                if (result.Errors.Count != 0)
                {
                    string errorMessage = "";
                    foreach (var error in result.Errors)
                    {
                        errorMessage += error.Description + " ";
                    }
                    TempData["ErrorMessage"] = errorMessage;
                    return RedirectToAction("Index", "Employee");
                }

                TempData["Success"] = "The employee has been deactivated.";
                return RedirectToAction("Index", "Employee");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction("Index", "Employee");
            }
        }


        #region Private Methods


        private ActionResult ShowError(Exception ex)
        {
            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Exception = ex
            });
        }


        private async Task<IEnumerable<SelectListItem>?> GetEmployeeList()
        {
            List<Employee> employees = await service.GetAllEmployeesAsync();

            return employees.Select(emp =>
            new SelectListItem
            {
                Value = emp.EmployeeId.ToString(),
                Text = emp.FullName,
            }).ToList();

        }

        private async Task<IEnumerable<SelectListItem>>? GetTypes()
        {
            var types = await new ListService().GetTypeScan();

            return types.Select(t =>
            new SelectListItem
            {
                Value = t.TypeId.ToString(),
                Text = t.TypeName
            }).ToList();
        }

        #endregion

    }
}
