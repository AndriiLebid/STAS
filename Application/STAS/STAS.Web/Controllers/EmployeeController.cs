﻿using Microsoft.AspNetCore.Http;
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
            // create classes and list of employees
            EmployeeWithScans employeeWithScans = new EmployeeWithScans();
            employeeWithScans.EmployeesList = await GetEmployeeList();
            employeeWithScans.ScanTypesList = await GetTypes();
            List<ScanVM> scansVm = new List<ScanVM>();

            try
            {
                if (employeeId == null)
                {
                    TempData["EmployeeEmpty"] = "Employee is not set.";
                    return View(employeeWithScans);
                }

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





        // GET: EmployeeController/Details/5
        public async Task<ActionResult> Details(int? id)
        {

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

        // GET: EmployeeController/Delete/5
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

        // POST: EmployeeController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}



        // GET: EmployeeController/Inactive/5
        //[HttpGet("Employee/Inactive/{id}")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Inactive(int id)
        //{
            
        //    try
        //    {
        //        Employee employee = await service.SearchEmployeeByIdAsync(id);

        //        if (employee == null) 
        //        {
        //            TempData["Error"] = "Employee not found.";
        //            return RedirectToAction("Index", "Employee");
        //        }

        //        employee.TypeEmployeeId = 1;

        //        employee = await service.UpdateEmployeeAsync(employee);

        //        TempData["Success"] = "The employee has been deactivated.";
        //        return RedirectToAction("Index", "Employee");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Error"] = ex.Message.ToString();
        //        return RedirectToAction("Index", "Employee");
        //    }

            
        //}


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
