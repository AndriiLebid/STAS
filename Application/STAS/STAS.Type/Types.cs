using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAS.Type
{
    /// <summary>
    /// Parameter for store procedure
    /// </summary>
    public class Parm
    {
        public string? Name { get; set; }
        public SqlDbType DataType { get; set; }
        public object? Value { get; set; }
        public int Size { get; set; }
        public ParameterDirection Direction { get; set; }

        public Parm(string? name, SqlDbType dataType, object? value, int size = 0, ParameterDirection direction = ParameterDirection.Input)
        {
            Name = name;
            DataType = dataType;
            Value = value;
            Size = size;
            Direction = direction;
        }
    }

    /// <summary>
    /// Error type, need for validation
    /// </summary>
    public enum ErrorType
    {
        Model, Business
    }

}
