using System.Collections.Generic;

namespace Entities
{
    public class PlanPayment
    {
        
        public int PlanID { get; set; }
        public List<ProductPayment> ProductPaymentDetails { get; set; } = new();
        public decimal ProspectusAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PaidTax { get; set; }
    }
}