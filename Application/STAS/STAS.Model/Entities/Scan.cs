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
        public int EmployeeId { get; set; }
        public DateTime ScanDate { get; set; }
        public int ScanType { get; set; }

    }
}
