using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CampaignDiscountMap
    {
        public int CampaignID { get; set; } 
        public decimal FromMarks { get; set; }
        public decimal ToMarks { get; set; }
        public DateTime CampaignDiscountMapValidTo { get; set; }
        public DateTime CampaignDiscountMapValidFrom { get; set; }
        public int CampaignDiscountMapstatus { get; set; }
        public string CampaignCouponName { get; set; }
        public string CampaignCouponPrefix { get; set; }
        public int CampaignCouponTypeID { get; set; }
        public int CampaignCouponcount { get; set; }
        public string CampaignMessageDesc { get; set; }

        public DiscountScheme DiscountSchemeDetail { get; set; } = new();
        

    }
}
