﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAS.Model
{
    public class Scan : BaseEntities
    {
  
        public int ScanId { get; set; }

        [Required(ErrorMessage = "Employee Id is required.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Scan Date is required.")]
        public DateTime ScanDate { get; set; }

        [Required(ErrorMessage = "Scan Type is required.")]
        public int ScanType { get; set; }

    }
}
