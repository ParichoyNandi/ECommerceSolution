using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateStudentPayOutPaymentDto
    {
        [Required]
        public string CustomerID { get; set; }

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

        [Required]
        public List<CreatePayOutFeeSchedulePaymentDto> FeeScheduleDetails { get; set; } = new();
    }
}
