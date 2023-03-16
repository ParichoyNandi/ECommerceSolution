using System.Collections.Generic;

namespace Entities
{
    public class ProductCenterMap
    {
        public int ProductID { get; set; }
        public int ProductCenterID { get; set; }
        public int CenterID { get; set; }
        public string CenterName { get; set; }
        public decimal ProspectusAmount { get; set; }
        public int StatusID { get; set; }
        public bool IsPublished { get; set; } = false;
        public List<ProductFeePlan> FeePlans { get; set; } = new List<ProductFeePlan>();
    }
}