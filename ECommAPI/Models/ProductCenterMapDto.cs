using System.Collections.Generic;

namespace ECommAPI.Models
{
    public class ProductCenterMapDto
    {
        public int ProductID { get; set; }
        public int ProductCenterID { get; set; }
        public int CenterID { get; set; }
        public string CenterName { get; set; }
        public decimal ProspectusAmount { get; set; }
        public List<ProductFeePlanDto> FeePlans { get; set; } = new List<ProductFeePlanDto>();
    }
}