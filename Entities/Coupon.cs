using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Coupon
    {
        public int CouponID { get; set; }
        public string CouponCode { get; set; }
        public string CouponName { get; set; }
        public string CouponDesc { get; set; }
        public int BrandID { get; set; }
        public DiscountScheme DiscountDetails { get; set; } = new();
        public CouponCategory CategoryDetails { get; set; } = new();
        public CouponType CouponTypeDetails { get; set; } = new();
        public int CouponCount { get; set; }
        public int PerStudentCount { get; set; } = 1;
        public int AssignedCount { get; set; }
        public decimal GreaterThanAmount { get; set; }
        public string CustomerCode { get; set; }
        public int StatusID { get; set; }
        public bool IsPrivate { get; set; } = false; //susmita:2022-09-12
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public List<Brand> BrandList { get; set; } = new();
    }
}
