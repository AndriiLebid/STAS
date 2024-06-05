using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAS.Web.Models
{
    public class Shift
    {
        public int SheeftId { get; set; }
        [Display(Name = "Employee Name")] 
        public string? EmployeeName { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [NotMapped]
        public TimeSpan Duration => EndDate - StartDate;

        [NotMapped]
        [Display(Name = "Shift Duration")]
        public string DurationFormatted => $"{Duration.Hours:D2}:{Duration.Minutes:D2}";





    }
}
