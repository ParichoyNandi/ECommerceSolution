using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetCampaignDetailsDto
    {
        public int CampaignID { get; set; }
        public string CampaignName { get; set; }
        public string CampaignDescription { get; set; }
        public int CampaignStatus { get; set; }
        public DateTime CampaignValidTo { get; set; }
        public DateTime CampaignValidFrom { get; set; }
        public int CampaignBrand { get; set; }
        public int CampaignBrandStatus { get; set; }

        public List<GetCampaignDiscountMapDetailsDto> campaignDiscountListsDetails { get; set; } = new List<GetCampaignDiscountMapDetailsDto>();

    }
}
