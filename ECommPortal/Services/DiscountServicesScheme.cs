using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class DiscountServicesScheme
    {

        public string DiscountSchemeName { get; set; }
        public DateTime validFrom { get; set; }
        public DateTime validTo { get; set; }
        public string BrandList { get; set; }

        [JsonProperty("DiscountSchemeDetails")]
        public List<DiscountServicesSchemeDetails> DiscountSchemeList { get; set; } = new();

        public string CreatedBy { get; set; } = "rice-group-admin";
    }
}
