using Microsoft.AspNetCore.Mvc.Rendering;
using STAS.Model;

namespace STAS.Web.Models
{
    public class ShiftVM
    {
        public List<Shift>? Shifts {  get; set; }

        public Employee? Employee { get; set; }

        public IEnumerable<SelectListItem>? EmployeesList { get; set; }
    }
}
