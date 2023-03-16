using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateStudentSubscriptionPaymentDto
    {
        [Required]
        public string CustomerID { get; set; }

        [Required]
        public int SubscriptionID { get; set; }

        [Required]
        public string TransactionNo { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string TransactionStatus { get; set; }

        [Required]
        public string TransactionSource { get; set; }

        [Required]
        public string TransactionMode { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal Tax { get; set; }

        [Required]
        public decimal TotalTransactionAmount { get; set; }
        public DateTime DueDate { get; set; } = DateTime.MinValue;
        public List<CreateSubscriptionFeeSchedulePaymentDto> FeeSchedulePaymentDetails { get; set; } = new();
    }
}
