using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetCouponDetailsWithCustomerIdDto
    {
        public int CouponID { get; set; }
        public string CouponCode { get; set; }
        public string CouponName { get; set; }
        public string CouponDesc { get; set; }
        public GetDiscountSchemeDto DiscountDetails { get; set; } = new();
        public GetCouponCategoryDto CategoryDetails { get; set; } = new();
        public GetCouponTypeDto CouponTypeDetails { get; set; } = new();
        public int StatusID { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public String CustomerCode { get; set; }

        public bool IsPrivate { get; set; }
    }
}
