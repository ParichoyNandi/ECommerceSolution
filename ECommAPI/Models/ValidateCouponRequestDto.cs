using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class ValidateCouponRequestDto
    {
        [Required]
        public List<ValidateCouponPlanRequestDto> PlanDetails { get; set; } = new();

        [Required]
        public string CouponCode { get; set; }
        public decimal PurchaseAmount { get; set; }
        public string CustomerID { get; set; }
    }
}
