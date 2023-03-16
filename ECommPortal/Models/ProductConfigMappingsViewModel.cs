using ECommPortal.Models.ValueObjects;
using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class ProductConfigMappingsViewModel : ResponseModel
    {
        public int ChoosenProductID { get; set; }
        public int ChoosenBrandId { get; set; }
        public string SummeryConfigValue { get; set; }
        public List<Config> ConfigLists { get; set; } = new();
        public List<Brand> BrandList { get; set; } = new();
        public List<Product> ProductLists { get; set; } = new();
        public List<Config> ChoosenConfigLists { get; set; } = new();
        public List<ProductSubHeaderProperty> SubHeaderLists { get; set; } = new();
        public ProductSubHeaderProperty ChoosenSubHeader { get; set; } = new();
        public List<ProductHeaderProperty> HeaderLists { get; set; } = new();
        public ProductHeaderProperty ChoosenHeader{ get; set; } = new();
        public SummaryFormat summarydetails { get; set; } = new();

        public IFormFile ProductStudyPlanIFile { get; set; }

    }
}
