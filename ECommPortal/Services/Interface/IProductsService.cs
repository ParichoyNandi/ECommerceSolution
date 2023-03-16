using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface IProductsService
    {
        Task<List<Brand>> BrandListsApi();
        Task<List<Course>> Courseapi(int brandid);
        Task<List<Category>> Categoryapi(int brandid);
        
        Task<string> ProductImageLink(IFormFile uploadedfile);

        Task<int> Productcreatemethod(ProductsServiceProperties pspobj);


    }
}
