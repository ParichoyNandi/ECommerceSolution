using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class StudentSubscriptionPaymentResponseDto
    {
        public int SubscriptionID { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionSource { get; set; }
        public string TransactionMode { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalTransactionAmount { get; set; }
        public DateTime DueDate { get; set; } = DateTime.MinValue;
        public List<StudentSubscriptionPaymentFeeScheduleResponseDto> FeeSchedulePaymentDetails { get; set; } = new();
    }
}
