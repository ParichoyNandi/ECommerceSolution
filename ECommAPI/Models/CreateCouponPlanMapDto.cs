using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateCouponPlanMapDto
    {
        [Required]
        public int CouponID { get; set; }

        [Required]
        public int PlanID { get; set; }
        public DateTime ValidFrom { get; set; } = DateTime.MinValue;
        public DateTime ValidTo { get; set; } = DateTime.MinValue;
    }
}
