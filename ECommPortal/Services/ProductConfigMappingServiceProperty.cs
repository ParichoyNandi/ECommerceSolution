using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class ProductConfigMappingServiceProperty
    {

        public int productID { get; set; }
        public int configID { get; set; }
        public string configValue { get; set; }
        public string configDisplayName { get; set; }
        public int subHeaderID { get; set; } = 0;
        public string subHeaderDisplayName { get; set; } = null;
        public int headerID { get; set; } = 0;
        public string headerDisplayName { get; set; } = null;
        

    }
}
