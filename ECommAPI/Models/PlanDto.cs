using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class PlanDto
    {
        public int PlanID { get; set; }
        public string PlanCode { get; set; }
        public string PlanName { get; set; }

        public int PlanLanguageID { get; set; } //susmita
        public string PlanLanguageName { get; set; } //susmita

        public bool IsPublished { get; set; }
        //public int BrandID { get; set; }
        public List<GetBrandDto> BrandDetails { get; set; } = new();
        public List<PlanConfigDto> ConfigList { get; set; } = new List<PlanConfigDto>();
        public List<int> CategoryIDList { get; set; } = new();
    }
}
