using System.ComponentModel.DataAnnotations;

namespace ECommAPI.Models
{
    public class ValidateCouponProductRequestDto
    {
        [Required]
        public int BrandID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int CenterID { get; set; }

        [Required]
        public string PaymentMode { get; set; }
    }
}