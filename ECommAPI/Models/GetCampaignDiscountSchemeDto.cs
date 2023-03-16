using System;

namespace ECommAPI.Models
{
    public class GetCampaignDiscountSchemeDto
    {
        public int DiscountSchemeID { get; set; }
        public string DiscountSchemeName { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

    }
}
