using System;
using System.Collections.Generic;

namespace Entities
{
    public class ProductFeePlan
    {
        public int ProductFeePlanID { get; set; }
        public int CourseFeePlanID { get; set; }
        public string FeePlan { get; set; }
        public string FeePlanDisplayName { get; set; }
        public decimal FeePlanAmount_Lumpsum { get; set; }
        public decimal FeePlanAmount_Instalment { get; set; }
        public int NumberOfInstalments { get; set; }
        public decimal FirstInstalmentAmount { get; set; }
        public decimal LumpsumTax { get; set; }
        public decimal InstalmentTax { get; set; }
        public decimal TotalInstalmentTax { get; set; }
        public int StatusID { get; set; }
        public bool IsPublished { get; set; } = false;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public List<InstalmentSummary> InstalmentSummary { get; set; } = new();
        public List<FeeStructure> FeePlanDetails { get; set; } = new();
    }
}