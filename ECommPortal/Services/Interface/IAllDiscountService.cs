using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface IAllDiscountService
    {
        Task<List<FeeComponent>> Feescomponentapi(int BrandID=0);

        Task<List<Brand>> BrandIdApi();
        Task<int> Discountcreatemethod(DiscountServicesScheme dssobj);
    }
}
