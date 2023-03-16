using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class DiscountMappingList
    {
        //public List<DiscountMappingProperty> DiscountMapList { get; set; } = new();

        public int DiscountSchemeID { get; set; }
        public int CenterID { get; set; }
        public int courseID { get; set; }
        public string createdBy { get; set; } = "rice-group-admin";
    }
}
