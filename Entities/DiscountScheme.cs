using System;
using System.Collections.Generic;

namespace Entities
{
    public class DiscountScheme
    {
        public int DiscountSchemeID { get; set; }
        public string DiscountSchemeName { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public List<DiscountSchemeDetail> DiscountSchemeDetails { get; set; } = new();
        public List<DiscountSchemeCenterMap> DiscountCenterList { get; set; } = new();
    }
}