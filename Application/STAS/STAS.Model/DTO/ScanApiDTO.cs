using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAS.Model
{
    public class ScanApiDTO /*: BaseEntities*/
    {
        [Required(ErrorMessage = "Employee Card Number is required.")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Employee Number must be 4 characters long.")]
        public string? EmployeeCardNumber { get; set; }
        [Required(ErrorMessage = "Scan Date is required.")]
        public DateTime ScanDate { get; set; }
        [Required(ErrorMessage = "Scan Type is required.")]
        public string? ScanType { get; set; }
    }
}
