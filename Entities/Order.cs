using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Order
    {
        public string CustomerID { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionMode { get; set; }
        public decimal TransactionAmount { get; set; }
        public string PaymentType { get; set; }
        public bool IsCompleted { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
