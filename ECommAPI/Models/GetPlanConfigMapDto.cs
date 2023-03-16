using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetPlanConfigMapDto
    {
        public int PlanConfigID { get; set; }
        public int PlanID { get; set; }
        public int ConfigID { get; set; }
        public string ConfigValue { get; set; }
        public string DisplayName { get; set; }
    }
}
