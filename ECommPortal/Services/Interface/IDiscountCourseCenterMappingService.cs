using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services.Interface
{
    public interface IDiscountCourseCenterMappingService
    {
        Task<List<DiscountScheme>> DiscountSchemeapi();
        Task<List<Course>> Courseapi();
        Task<List<Centre>> Centerapi();
        Task<Responsestatus> DiscountMappingmethod(List<DiscountMappingProperty> dmlobj);
    }
}
