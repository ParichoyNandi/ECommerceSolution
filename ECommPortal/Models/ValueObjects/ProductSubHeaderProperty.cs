using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models.ValueObjects
{
    public class ProductSubHeaderProperty
    {

        public int SubHeaderID { get; set; } 
        public string SubHeaderCode { get; set; }
        public string SubHeaderDisplayName { get; set; }
    }
}
