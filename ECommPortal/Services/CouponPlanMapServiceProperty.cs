using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class CouponPlanMapServiceProperty
    {
        public int couponID { get; set; }
        public int planID { get; set; }
        public DateTime validFrom { get; set; }
        public DateTime validTo { get; set; }
    }
}
