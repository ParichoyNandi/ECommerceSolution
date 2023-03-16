using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class CreateCouponPlanMapModel:ResponseModel
    {
        public List<CouponCategory> CouponCategoryList { get; set; } = new();
        public List<CouponType> CouponTypeList { get; set; } = new();
        public List<Plan> PlanList { get; set; } = new();

        public List<Coupon> CouponList { get; set; } = new();

        public List<ChoosedPlan> ChoosedPlanList { get; set; } = new();

        public List<Brand> BrandList { get; set; } = new();
        public int couponId { get; set; }
        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }
        public int ChoosenBranID { get; set; }

    }
    public class ChoosedPlan
    {
        public int ChoosedPlanID { get; set; }

        public bool statusvalue { get; set; }
        public string ChoosedPlanName { get; set; }

    }


}
