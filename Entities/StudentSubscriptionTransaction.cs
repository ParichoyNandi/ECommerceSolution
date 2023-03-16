using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class StudentSubscriptionTransaction
    {
        public int SubscriptionTransactionID { get; set; }
        public int SubscriptionID { get; set; }
        public int BrandID { get; set; }
        public string StudentID { get; set; }
        public int StudentDetailID { get; set; }
        public int CenterID { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionSource { get; set; }
        public string TransactionMode { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int ReceiptHeaderID { get; set; }
        public int FeeScheduleID { get; set; }
        public string CustomerID { get; set; }
    }
}
