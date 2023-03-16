using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommAPI.Models
{
    public class ValidateCouponPlanRequestDto
    {
        [Required]
        public int PlanID { get; set; }

        [Required]
        public List<ValidateCouponProductRequestDto> ProductDetails { get; set; } = new();
    }
}