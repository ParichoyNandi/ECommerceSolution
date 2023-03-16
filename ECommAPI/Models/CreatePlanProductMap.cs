using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreatePlanProductMap
    {
        public int PlanID { get; set; }
        public List<int> ProductID { get; set; }
    }
}
