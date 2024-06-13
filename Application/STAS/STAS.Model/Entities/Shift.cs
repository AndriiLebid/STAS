using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAS.Model
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
        public TimeSpan DurationLunch => (EndLunch < StartLunch) ? TimeSpan.Zero : (EndLunch - StartLunch);

        [NotMapped]
        public TimeSpan DurationFull => EndDate - StartDate;

        [NotMapped]
        public TimeSpan Duration => DurationFull - DurationLunch;

        [NotMapped]
        [Display(Name = "Shift Duration")]
        public string DurationFormatted => $"{(Duration.Days*12 + Duration.Hours):D2}:{Duration.Minutes:D2}";





    }
}
