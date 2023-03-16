using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreatePaymentDto
    {
        [Required]
        public string CustomerID { get; set; }

        [Required]
        public List<CreatePlanPaymentDto> PlanPaymentDetails { get; set; } = new();

        //public string Address { get; set; }
        //public int CityID { get; set; }
        //public int StateID { get; set; }
        //public int CountryID { get; set; }
        //public string Pincode { get; set; }

        [Required]
        public string TransactionNo { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string TransactionSource { get; set; }

        [Required]
        public string TransactionMode { get; set; }

        [Required]
        public string TransactionStatus { get; set; }

        [Required]
        public decimal PaidAmount { get; set; }

        [Required]
        public decimal PaidTax { get; set; }
    }
}
