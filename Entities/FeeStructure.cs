using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class FeeStructure
    {
        public int CourseFeePlanDetailID { get; set; }
        public int FeeComponentID { get; set; }
        public int FeePlanID { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount 
        {
            get
            {
                return Amount - DiscountAmount;
            }
        }
        public int InstalmentNo { get; set; }
        public DateTime InstalmentDate { get; set; }
        public int Sequence { get; set; }
        public string IsLumpsum { get; set; }
        public int DisplayFeeComponentID { get; set; }
        public List<FeeStructureTax> TaxDetails { get; set; } = new();
    }
}
