using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ProductSubscription
    {
        public string CustomerID { get; set; }
        public int SubscriptionID { get; set; }
        public string TransactionNo { get; set; }
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public int ProductID { get; set; }
        public string SubscriptionPlanID { get; set; }
        public string Authkey { get; set; }
        public int BillingPeriod { get; set; }
        public DateTime BillingStartDate { get; set; }
        public DateTime BillingEndDate 
        {
            get
            {
                if (BillingPeriod <= 0 && Authkey!=null && Authkey!="")
                    throw new Exception("Billing Period cannot be less than or equal to zero");


                return BillingStartDate.AddMonths(BillingPeriod);
            }
        }
        public decimal TotalBillingAmount { get; set; }
        public string SubscriptionStatus { get; set; }
        public string MandateLink { get; set; }
    }
}
