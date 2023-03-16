using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class PlanConfigMappingServiceProperty
    {
        public int planID { get; set; }
        public int configID { get; set; }
        public string configValue { get; set; }
        public string displayName { get; set; }
    }
}
