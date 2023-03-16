using ECommPortal.Models.ValueObjects;
using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface IPlanProductsConfigMappingService
    {
        Task<List<Brand>> BrandListsApi();
        Task<List<Plan>> GetPlanListsApi(string BrandLists);
        Task<List<Product>> GetProductsapi(int brandid, Boolean Ispublished);
        Task<List<Config>> ConfigListsApi();
        Task<List<PlanHeaderProperty>> GetPlanHeaderApi();
        Task<List<PlanSubHeaderProperty>> GetPlanSubHeaderApi();
        Task<string> StudyPlanDownloadLink(IFormFile uploadedfile);
        Task<Responsestatus> SavePlanConfigMapping(List<PlanConfigMappingServiceProperty> ppcsp);

        Task<Responsestatus> SavePlanproductMapping(PlanProductsMappingServiceProperty ppmsp);
    }
}
