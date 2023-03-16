using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class Response<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T data { get; set; }
    }
}
