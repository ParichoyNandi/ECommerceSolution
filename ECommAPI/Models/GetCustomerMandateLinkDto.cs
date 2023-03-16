using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetCustomerMandateLinkDto
    {
        public string CustomerID { get; set; }
        public string TransactionNo { get; set; }
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public string MandateLink { get; set; }
    }
}
