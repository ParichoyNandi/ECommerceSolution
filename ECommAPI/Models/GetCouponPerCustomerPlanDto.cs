using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetCouponPerCustomerPlanDto
    {
        public int PlanID { get; set; } = 0;
        public string CustomerID { get; set; } = null;
        public List<GetCouponDetailsWithCustomerIdDto> Coupons { get; set; } = new();
    }
}
