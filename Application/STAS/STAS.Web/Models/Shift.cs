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

        [Display(Name = "Start Break")]
        public DateTime StartBreak { get; set; }

        [Display(Name = "End Break")]
        public DateTime EndBreak { get; set; }

        [Display(Name = "Start Lunch")]
        public DateTime StartLunch { get; set; }

        [Display(Name = "End Lunch")]
        public DateTime EndLunch { get; set; }

        [NotMapped]
        public TimeSpan DurationLunch => EndLunch - StartLunch;

        [NotMapped]
        public TimeSpan Duration => EndDate - StartDate - DurationLunch;

        [NotMapped]
        [Display(Name = "Shift Duration")]
        public string DurationFormatted => $"{(Duration.Days*12 + Duration.Hours):D2}:{Duration.Minutes:D2}";





    }
}
