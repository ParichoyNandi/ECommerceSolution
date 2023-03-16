using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface IPlanService
    {
        Task<List<Brand>> BrandListsApi();
        Task<string> PlanImageLink(IFormFile uploadedfile);

        Task<int> Plancreatemethod(PlanServiceProperty pspobj);
    }
}
