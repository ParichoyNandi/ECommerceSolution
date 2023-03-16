using ECommPortal.Models.ValueObjects;
using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface IProductConfigMappingService
    {
        Task<List<Product>> GetProductsapi(int brandid,Boolean Ispublished);
        Task<List<Brand>> BrandListsApi();
        Task<List<Config>> ConfigListsApi();
        Task<List<ProductHeaderProperty>> GetProductHeaderApi();
        Task<List<ProductSubHeaderProperty>> GetProductSubHeaderApi();
        Task<string> StudyPlanDownloadLink(IFormFile uploadedfile);
        Task<Responsestatus> ProductconfigMap(List<ProductConfigMappingServiceProperty> pcmsp);

       

    }
}
