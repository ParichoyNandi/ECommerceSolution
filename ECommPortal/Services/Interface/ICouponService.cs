using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface ICouponService
    {
        Task<List<CouponCategory>> CouponCategoryapi();
        Task<List<DiscountScheme>> DiscountSchemeapi(string BrandLists);
        Task<List<Brand>> BrandListsApi();
        Task<int> Couponcreatemethod(CouponServiceProperties cs);
    }
}
