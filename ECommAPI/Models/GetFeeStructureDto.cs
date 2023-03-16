using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetFeeStructureDto
    {
        public int CourseFeePlanDetailID { get; set; }
        public int FeeComponentID { get; set; }
        public int FeePlanID { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public int InstalmentNo { get; set; }
        public DateTime InstalmentDate { get; set; }
        public int Sequence { get; set; }
        public string IsLumpsum { get; set; }
        public int DisplayFeeComponentID { get; set; }
        public List<GetFeeStructureTaxDto> TaxDetails { get; set; } = new();
    }
}
