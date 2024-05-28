using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STAS.Type;

namespace STAS.Model
{
    public class ValidationError
    {
        public ValidationError(string? desc, ErrorType? errType)
        {
            Description = desc;
            ErrorType = errType;
        }
        public string? Description { get; set; }
        public ErrorType? ErrorType { get; set; }

    }
}
