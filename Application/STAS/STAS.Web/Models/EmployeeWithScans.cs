using STAS.Model;
using System.Data;

namespace STAS.Web.Models
{
    public class EmployeeWithScans
    {
        public Employee? Employee { get; set; }

        public List<ScanVM>? Scans { get; set; }
    }
}

    public class ScanVM
    {

        public int ScanId { get; set; }
        public DateTime ScanDate { get; set; }
        public string? ScanType { get; set; }

}

