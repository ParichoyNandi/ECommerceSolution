using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetConfigDto
    {
        public int ConfigID { get; set; }
        public string ConfigCode { get; set; }
        public string ConfigName { get; set; }
        public string ConfigDefaultValue { get; set; }
    }
}
