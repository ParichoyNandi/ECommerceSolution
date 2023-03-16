using ECommPortal.Models.ValueObjects;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class ProductMappingsViewModel : ResponseModel
    {
        public int ProductID { get; set; }

        public int ChoosenBrandId { get; set; }
        public List<Brand> BrandList { get; set; } = new();
        public List<Centre> CenterLists { get; set; } = new();
        public List<Product> ProductLists { get; set; } = new();
        public List<ProductFeePlan> ProductFeePlanLists { get; set; } = new();
        public List<ExamCategory> ExamCategoryLists { get; set; } = new();
        public List<ChoosenCentreFeePlanMap> ChoosenCentreFeePlanLists { get; set; } = new();
        public List<int> ChoosenExamCategoryLists { get; set; }

       

    }
}
