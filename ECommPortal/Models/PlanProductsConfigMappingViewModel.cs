using ECommPortal.Models.ValueObjects;
using Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class PlanProductsConfigMappingViewModel : ResponseModel
    {
        public List<Brand> BrandLists { get; set; } = new();
        public int ChoosenBrandID { get; set; } = new();
        public int ChoosenPlanID { get; set; } = new();
        public string SummeryConfigValue { get; set; }
        public List<Plan> PlanLists { get; set; } = new();
        public List<Product> ProductLists { get; set; } = new();
        public List<Config> ConfigLists { get; set; } = new();
        public List<ChoosenProductDetails> ChoosenProductLists { get; set; } = new();
        public List<Config> ChoosenConfigLists { get; set; } = new();
        public List<PlanSubHeaderProperty> SubHeaderLists { get; set; } = new();
        public PlanSubHeaderProperty ChoosenSubHeader { get; set; } = new();
        public List<PlanHeaderProperty> HeaderLists { get; set; } = new();
        public PlanHeaderProperty ChoosenHeader { get; set; } = new();
        public SummaryFormat summarydetails { get; set; } = new();
        public IFormFile ProductStudyPlanIFile { get; set; }


    }
}
