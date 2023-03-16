using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetProductConfigMapDto
    {
        public int ProductConfigID { get; set; }
        public int ProductID { get; set; }
        public int ConfigID { get; set; }
        public string ConfigCode { get; set; }
        public string ConfigValue { get; set; }
        public string ConfigDisplayName { get; set; }
        public int SubHeaderID { get; set; }
        public string SubHeaderDisplayName { get; set; }
        public int HeaderID { get; set; }
        public string HeaderDisplayName { get; set; }
    }
}
