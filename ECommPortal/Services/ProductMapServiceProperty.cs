using ECommPortal.Services.ValueObjectsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Services
{
    public class ProductMapServiceProperty
    {
        public int productID { get; set; }
        public String examCategoryList { get; set; }

        public String createdBy { get; set; } = "rice-group-admin";
        public List<CentreFeePlanMapServiceProperty> centerFeePlanDetails { get; set; } = new();

    }
}
