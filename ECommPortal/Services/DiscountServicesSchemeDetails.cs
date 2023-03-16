using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class DiscountServicesSchemeDetails
    {
        public int DiscountRate { get; set; }
        public int DiscountAmount { get; set; }

        public int isApplicableOn { get; set; }

        [JsonProperty("FromInstalment")]
        public string fromInstallment { get; set; }
        public string feeComponentID { get; set; }
    }
}
