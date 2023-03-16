using Entities;
using System;
using System.Collections.Generic;

namespace ECommPortal.Models
{
    public class PlanListDashboardViewModel
    {
        public String publication { get; set; }
        public int ChoosenBrandID { get; set; }
        public int ChoosenLanguageID { get; set; }
        public List<Brand> BrandList { get; set; } = new();
        public List<Plan> PlanLists { get; set; } = new();
        public Product ProductDetails { get; set; } = new();
        public int ChoosenPlanId { get; set; }
        public Plan GetPlanDetails {   get; set; }
        public int UpdatePlanDetails { get; set; }
        
    }
}
