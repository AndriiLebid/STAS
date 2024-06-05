using Microsoft.AspNetCore.Mvc.Rendering;
using STAS.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STAS.Web.Models
{
    public class ShiftVM
    {
        public List<Shift>? Shifts {  get; set; }

        public Employee? Employee { get; set; }

        public TimeSpan TotalDuration { get; set; }

        [NotMapped]
        [Display(Name = "Shift Duration")]
        public string TotalDurationFormatted => $"{TotalDuration.Hours:D2}:{TotalDuration.Minutes:D2}";

        public IEnumerable<SelectListItem>? EmployeesList { get; set; }
    }
}
