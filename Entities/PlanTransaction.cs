using System.Collections.Generic;

namespace Entities
{
    public class PlanTransaction
    {
        public int TransactionPlanDetailsID { get; set; }
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public bool CanBeProcessed { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
        public List<ProductTransaction> ProductDetails { get; set; } = new();
    }
}