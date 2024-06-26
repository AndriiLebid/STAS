﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STAS.Services;
using STAS.Web.Models;
using STAS.Model;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace STAS.Web.Controllers
{

    public class ScanController : Controller
    {
        private readonly EmployeeService empService = new();
        private readonly ScanService scanService = new();
        private readonly ListService listService = new();


        // GET: ScanController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: ScanController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: ScanController/Create
        public async Task<ActionResult> Create(int? id)
        {

            //check login
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return RedirectToAction("Login", "Login");
            }

            ScanAddEditVM scanVM = new();
            try
            {
                if (id == null) new Exception("No parameter in the request");

                
                Employee employee = await empService.SearchEmployeeByIdAsync((int)id);

                if (employee == null) new Exception("Wrong employee id in the request");

                scanVM.scan.ScanDate = DateTime.Now;
                scanVM.scan.EmployeeId = (int)id;
                scanVM.EmployeeName = employee!.FullName;
                scanVM.ScanTypesList = await GetTypes();

                return View(scanVM);

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction("Details", "Employee", new { id = scanVM.scan.EmployeeId });
            }
        }

        // POST: ScanController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ScanAddEditVM scanVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    scanVM.ScanTypesList = await GetTypes();
                    return View(scanVM);
                }

                Scan scan = new Scan
                {
                    ScanDate = scanVM.scan.ScanDate,
                    EmployeeId = scanVM.scan.EmployeeId,
                    ScanType = scanVM.scan.ScanType
                };


                var result = await scanService.AddScanRecordAsync(scan);

                if (result.Errors.Count != 0)
                {
                    string errorMessage = "";
                    foreach (var error in result.Errors)
                    {
                        errorMessage += error.Description + " ";
                    }
                    ViewBag.ErrorMessage = errorMessage;
                    return View(scan);
                }

                TempData["Success"] = "Scan created successfully.";
                return RedirectToAction("Details", "Employee", new { id = scan.EmployeeId });

            }
            catch(Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction("Details", "Employee", new { id = scanVM.scan.EmployeeId });
            }
        }

        // GET: ScanController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            //check login
            var userName = HttpContext.Session.GetString("UserName");

            if (userName == null)
            {
                return RedirectToAction("Login", "Login");
            }

            Scan scan = new();
            try
            {
                if (id == null) new Exception("No parameter in the request");

                ScanAddEditVM scanVM = new();
                scan = await scanService.GetScanByIdAsync((int)id!);

                if (scan == null) new Exception("Wrong parameter in the request");

                scanVM.scan.ScanId = scan!.ScanId;
                scanVM.scan.ScanDate = scan.ScanDate;
                scanVM.scan.EmployeeId = scan.EmployeeId;
                Employee emp = await empService.SearchEmployeeByIdAsync(scan.EmployeeId);
                scanVM.EmployeeName = emp.FullName;
                scanVM.scan.ScanType = scan.ScanType;
                scanVM.scan.RecordVersion = scan.RecordVersion;
                
                scanVM.ScanTypesList = await GetTypes();

                return View(scanVM);

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction("Details", "Employee", new { id = scan.EmployeeId });
            }
        }

        // POST: ScanController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ScanAddEditVM scanVM)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View(scanVM);
                }

                Scan scan = new Scan
                {
                    
                    ScanId = scanVM.scan.ScanId,
                    ScanDate = scanVM.scan.ScanDate,
                    EmployeeId = scanVM.scan.EmployeeId,
                    ScanType = scanVM.scan.ScanType,
                    RecordVersion = scanVM.scan.RecordVersion
                    
                };


                var result = await scanService.UpdateRecordAsync(scan);

                if (result.Errors.Count != 0)
                {
                    string errorMessage = "";
                    foreach (var error in result.Errors)
                    {
                        errorMessage += error.Description + " ";
                    }
                    ViewBag.ErrorMessage = errorMessage;
                    return View(scan);
                }

                TempData["Success"] = "Scan created successfully.";
                return RedirectToAction("Details", "Employee", new { id = scan.EmployeeId });


            }
            catch(Exception ex) 
            {
                TempData["Error"] = ex.Message.ToString();
                return RedirectToAction("Details", "Employee", new { id = scanVM.scan.EmployeeId });
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
