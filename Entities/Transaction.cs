using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public string TransactionNo { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string TransactionStatus { get; set; }
        public bool CanBeProcessed { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
        public int StatusID { get; set; }
        public List<PlanTransaction> PlanDetails { get; set; } = new();
    }
}
