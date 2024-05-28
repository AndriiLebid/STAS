using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STAS.Type
{

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

    public enum ErrorType
    {
        Model, Business
    }

}
