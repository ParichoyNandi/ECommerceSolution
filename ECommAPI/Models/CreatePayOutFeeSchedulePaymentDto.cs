using System.ComponentModel.DataAnnotations;

namespace ECommAPI.Models
{
    public class CreatePayOutFeeSchedulePaymentDto
    {
        [Required]
        public int FeeScheduleID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public decimal Tax { get; set; }
    }
}