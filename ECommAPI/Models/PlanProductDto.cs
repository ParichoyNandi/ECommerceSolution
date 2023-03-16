using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class PlanProductDto
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        //public int PlanProductID { get; set; }
        public int PlanLanguageID { get; set; } //susmita
        public string PlanLanguageName { get; set; }//susmita
        public List<ProductDto> ProductDetails { get; set; } = new List<ProductDto>();
        public List<ProductCenterMapDto> ProductCentreDetails { get; set; } = new List<ProductCenterMapDto>();
        public List<PlanConfigDto> PlanConfigList { get; set; } = new();
        public GetSummaryFormatDto SummaryDetails { get; set; } = new();
    }
}
