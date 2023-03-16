using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models.ValueObjects
{
    public class ProductHeaderProperty
    {
        public int HeaderID { get; set; }
        public string HeaderCode { get; set; }
        public string HeaderDisplayName { get; set; }
    }
}
