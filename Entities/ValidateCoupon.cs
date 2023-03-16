using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ValidateCoupon
    {
        public int PlanID { get; set; }
        public int ProductID { get; set; }
        public string PaymentMode { get; set; }
        public int CenterID { get; set; }
        public bool IsCouponApplicable { get; set; } = false;
        public decimal PayableAmount { get; set; }
        public decimal PayableTax { get; set; }
        public decimal TotalPayableInstalmentAmount { get; set; }
        public decimal TotalPayableInstalmenTaxtAmount { get; set; }
    }
}
