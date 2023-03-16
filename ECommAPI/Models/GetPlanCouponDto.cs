using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetPlanCouponDto
    {
        public int PlanID { get; set; }
        public List<GetCouponDto> Coupons { get; set; } = new();
    }
}
