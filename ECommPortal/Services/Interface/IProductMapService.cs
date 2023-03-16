using ECommPortal.Services.ValueObjectsService;
using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface IProductMapService
    {
        Task<List<Centre>> ProductCenterapi(int brandid);
        Task<List<Product>> GetProductsapi(int brandid, Boolean Ispublished);
        Task<List<ExamCategory>> GetExamCategoryapi(int brandid);
        Task<List<Brand>> BrandListsApi();
        Task<List<ProductFeePlanProperty>> GetFeePlanapi(int centerid,int productid);
        Task<Responsestatus> SaveProductMapping(ProductMapServiceProperty pmsp);
    }
}
