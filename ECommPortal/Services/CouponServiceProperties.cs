using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class CouponServiceProperties
    {
        public string couponCode { get; set; }
        public string couponName { get; set; }
        public string couponDesc { get; set; }
        public int discountID { get; set; }
        public int couponCategoryID { get; set; }
        public int couponTypeID { get; set; }
        public int couponCount { get; set; }
        public int greaterThanAmount { get; set; }
        public string customerCode { get; set; }
        public DateTime validFrom { get; set; }
        public DateTime validTo { get; set; }
        public string brandList { get; set; }
        public bool isPrivate { get; set; } //susmita : 2022-09-12

    }
}
