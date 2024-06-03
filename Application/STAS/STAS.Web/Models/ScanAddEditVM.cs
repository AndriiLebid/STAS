using Microsoft.AspNetCore.Mvc.Rendering;
using STAS.Model;

namespace STAS.Web.Models
{
    public class ScanAddEditVM
    {
        public Scan scan { get; set; } = new();

        public string? EmployeeName { get; set; }

        public IEnumerable<SelectListItem>? ScanTupesList { get; set; } 
    }
}
