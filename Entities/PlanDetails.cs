using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PlanDetails
    {
        public int PlanDetailID { get; set; }
        public PlanProduct PlanProduct { get; set; } = new PlanProduct();
        public string PaymentMode { get; set; }
        public ProductFeePlan ProductFeePlan { get; set; } = new ProductFeePlan();

    }
}
