using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class FeescomponentVariable<T>
    {
       
            public string Status { get; set; }
            public string Message { get; set; }
            public T data { get; set; }
        
    }
}
