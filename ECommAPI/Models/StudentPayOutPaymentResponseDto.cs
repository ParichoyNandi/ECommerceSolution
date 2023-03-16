using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class StudentPayOutPaymentResponseDto
    {
        public string CustomerID { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionSource { get; set; }
        public string TransactionMode { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalTransactionAmount { get; set; }
        public List<StudentPayOutPaymentFeeScheduleResponseDto> FeeScheduleDetails { get; set; } = new();
    }
}
