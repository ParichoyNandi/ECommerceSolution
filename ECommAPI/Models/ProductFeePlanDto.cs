using System.Collections.Generic;

namespace ECommAPI.Models
{
    public class ProductFeePlanDto
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
        public List<GetInstalmentSummaryDto> InstalmentSummary { get; set; } = new();
        //public List<GetFeeStructureDto> FeePlanDetails { get; set; } = new();
    }
}