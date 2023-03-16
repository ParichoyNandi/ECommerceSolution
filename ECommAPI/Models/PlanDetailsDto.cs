using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class PlanDetailsDto
    {
        public int PlanDetailID { get; set; }
        public PlanProductDto PlanProduct { get; set; } = new PlanProductDto();
        public string PaymentMode { get; set; }
        public ProductFeePlanDto ProductFeePlan { get; set; } = new ProductFeePlanDto();
    }
}
