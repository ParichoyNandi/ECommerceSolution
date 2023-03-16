using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface IPlanPublishDeactiveService
    {
        Task<List<Plan>> GetPublishedDeactivetedPlansapi(string BrandList);
        Task<List<Brand>> BrandListsApi();
        Task<Responsestatus> ModifyPlanIsPublishapi(int planid, Boolean IsPublished);
    }
}
