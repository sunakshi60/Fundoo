using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model
{
    public class ResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }
    }
}
