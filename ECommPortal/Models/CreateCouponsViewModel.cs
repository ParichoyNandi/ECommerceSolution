using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class CreateCouponsViewModel:ResponseModel
    {
        public Coupon CouponBasicDetailsList { get; set; } = new();
       
        public int DiscountId { get; set; }
        public List<Brand> BrandLists { get; set; } = new();
        public List<int> ChoosenBrandLists { get; set; } = new();
        public List<DiscountScheme> DiscountSchemeForcoupon { get; set; } = new();
        public List<CouponCategory> CouponCategoryList { get; set; } = new();
        public List<CouponType> CouponTypeList { get; set; } = new();

       

    }
}
