using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CampaignCoupon
    {
        public string CouponCode { get; set; }
        public decimal LumpsumDiscountPerc { get; set; }
        public decimal InstalmentDiscountPerc { get; set; }
        public string MessageDesc { get; set; }
    }
}
