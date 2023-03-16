using ECommPortal.Services.ValueObjectsService;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ECommPortal.Services.Interface
{
    public interface IProductListDashboardService
    {
        Task<List<Product>> GetProductsapi(int brandid, Boolean Ispublished);


    }
}
