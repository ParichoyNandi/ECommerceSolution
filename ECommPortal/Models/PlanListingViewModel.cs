using ECommPortal.Models.ValueObjects;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class PlanListingViewModel : ResponseModel
    {
        public string choosenBrandListString { get; set; }
        public List<Brand> BrandLists { get; set; } = new();
        public List<int> ChoosenBrandLists { get; set; } = new();
        public List<Plan> PlanLists { get; set; } = new();
        public List<Plan> ChoosedUpdatedPlanLists { get; set; } = new();

    }
}
