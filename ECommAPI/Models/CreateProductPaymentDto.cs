using System.ComponentModel.DataAnnotations;

namespace ECommAPI.Models
{
    public class CreateProductPaymentDto
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public int BatchID { get; set; }

        [Required]
        public int ProductCentreID { get; set; }

        [Required]
        public int PaymentMode { get; set; }
        public string CouponCode { get; set; }

        [Required]
        public int ProductFeePlanID { get; set; }
        public decimal ProspectusAmount { get; set; }

        [Required]
        public decimal PaidAmount { get; set; }

        [Required]
        public decimal PaidTax { get; set; }

        public CreateSubscriptionDto SubscriptionDetails { get; set; } = new();

    }
}