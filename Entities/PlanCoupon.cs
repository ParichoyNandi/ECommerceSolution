using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PlanCoupon
    {
        public int PlanID { get; set; }
        public List<Coupon> Coupons { get; set; } = new();
    }
}
