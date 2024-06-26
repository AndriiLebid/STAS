﻿using Microsoft.AspNetCore.Mvc.Rendering;
using STAS.Model;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace STAS.Web.Models
{
    public class EmployeeWithScans
    {
        public Employee? Employee { get; set; }

        public List<ScanVM>? Scans { get; set; }

        public IEnumerable<SelectListItem>? EmployeesList { get; set; }
        public IEnumerable<SelectListItem>? ScanTypesList { get; set; }
    }
}
