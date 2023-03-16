using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PlanProduct
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public int PlanLanguageID { get; set; } //susmita
        public string PlanLanguageName { get; set; }//susmita
        public int PlanProductID { get; set; }
        public Product ProductDetails { get; set; } = new Product();
        public List<PlanConfig> PlanConfigList { get; set; } = new();
    }
}
