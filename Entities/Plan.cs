using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Plan
    {
        public int PlanID { get; set; }
        public string PlanCode { get; set; }
        public string PlanName { get; set; }
        public string PlanDesc { get; set; }
        public string BrandIDList { get; set; }
        //public int BrandID { get; set; }
        public List<Brand> BrandDetails { get; set; } = new();
        public bool IsPublished { get; set; }
        public int StatusID { get; set; }
        public string PlanImage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public List<PlanConfig> ConfigList { get; set; } = new List<PlanConfig>();
        public List<int> CategoryIDList { get; set; } = new();

        public int PlanLanguageID { get; set; } //susmita
        public string PlanLanguageName { get; set; } //susmita

        //ParichoyIntern
        public string PlanCourseDuration { get; set; }
        public string PlanPrice { get; set; }
        public string PlanDiscountedPrice { get; set; }

    }
}
