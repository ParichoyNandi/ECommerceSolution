using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface IPlanCouponMapService
    {
        Task<List<CouponCategory>> CouponCategoryapi();
        Task<List<Brand>> BrandListsApi();
        Task<List<Plan>> Planapi(int couponid, int brandid);
        Task<List<Coupon>> Couponapi(int id,int brandid);
        Task<Response<int>> CouponPlanapi(List<CouponPlanMapServiceProperty> cpsp);
    }
}
