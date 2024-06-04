using System;
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
        [Display(Name = "Employee Id")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Scan Date is required.")]
        [Display(Name = "Scan Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ScanDate { get; set; }

        [Required(ErrorMessage = "Scan Type is required.")]
        [Display(Name = "Scan Type")]
        public int ScanType { get; set; }
        public byte[]? RecordVersion { get; set; }

    }
}
