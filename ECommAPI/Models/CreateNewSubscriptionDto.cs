using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateNewSubscriptionDto
    {
        [Required]
        public string TransactionNo { get; set; }

        [Required]
        public int PlanID { get; set; }

        [Required]
        public int ProductID { get; set; }
        public string SubscriptionPlanID { get; set; }
        public string Authkey { get; set; }
        public int BillingPeriod { get; set; }
        public DateTime BillingStartDate { get; set; }
        public DateTime BillingEndDate { get; set; }
        public decimal TotalBillingAmount { get; set; }

        [Required]
        public string SubscriptionStatus { get; set; }
        public string MandateLink { get; set; }
        public int SubscriptionID { get; set; }
    }
}
