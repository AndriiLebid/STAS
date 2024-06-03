using System.ComponentModel.DataAnnotations;

namespace STAS.Web.Models
{
    public class ScanVM
    {
        [Display(Name = "Scan Id")]
        public int ScanId { get; set; }

        public int EmployeeId { get; set; }

        public string? FullName { get; set; }    

        [Display(Name = "Scan Date")]
        public DateTime ScanDate { get; set; }

        [Display(Name = "Scan Type")]
        public string? ScanType { get; set; }

    }

}
