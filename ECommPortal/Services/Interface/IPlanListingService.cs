using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface IPlanListingService
    {
        Task<List<Brand>> BrandListsApi();
        Task<List<Plan>> GetPlanListsApi(string BrandLists);
    }
}
