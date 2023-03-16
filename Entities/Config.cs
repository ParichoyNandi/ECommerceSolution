using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Config
    {
        public int ConfigID { get; set; }
        public string ConfigCode { get; set; }
        public string ConfigName { get; set; }
        public string ConfigDefaultValue { get; set; }
        public int StatusID { get; set; }
    }
}
