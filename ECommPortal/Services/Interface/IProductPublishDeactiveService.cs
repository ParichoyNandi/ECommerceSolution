using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface IProductPublishDeactiveService
    {
        Task<List<Product>> GetPublishedDeactivetedProductsapi(int brandid);
        Task<List<Brand>> BrandListsApi();
        Task<List<Product>> GetProductsapi(int brandid, Boolean Ispublished);
        Task<Responsestatus> ModifyProductIsPublishapi(int productid, Boolean IsPublished);

       
    }
}
