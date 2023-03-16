using System;

namespace ECommAPI.Models
{
    public class CreateSubscriptionDto
    {
        public string SubscriptionPlanID { get; set; }
        public string Authkey { get; set; }
        public int BillingPeriod { get; set; }
        public DateTime BillingStartDate { get; set; }
        public decimal TotalBillingAmount { get; set; }
    }
}