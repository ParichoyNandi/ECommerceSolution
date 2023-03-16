using ECommPortal.Models.ValueObjects;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class PlanPublishDeactiveViewModel : ResponseModel
    {
        public List<int> ChoosenBrandLists { get; set; }
        public List<Brand> BrandLists { get; set; } = new();
        public List<Plan> PlanLists { get; set; } = new();
        public string ChoosenBrandListString { get; set; }

        public List<ChoosenPlanForActiveDeactive> ChoosenPlanLists { get; set; } = new();
    }
}
