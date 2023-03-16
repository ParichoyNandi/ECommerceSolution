using ECommPortal.Services.ValueObjectsService;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ECommPortal.Services.Interface
{
    public interface IPlanListDashboardService
    {
        Task<List<Plan>> GetPlansapi(String brandid, int language);
        Task<List<Brand>> BrandListsApi();
        Task <Plan> GetPlansDetailsApi(int PlanID);

    }
}
