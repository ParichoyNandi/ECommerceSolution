using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommAPI.Models
{
    public class CreatePlanPaymentDto
    {
        [Required]
        public int PlanID { get; set; }

        [Required]
        public List<CreateProductPaymentDto> ProductPaymentDetails { get; set; } = new();
        public decimal ProspectusAmount { get; set; }

        [Required]
        public decimal PaidAmount { get; set; }

        [Required]
        public decimal PaidTax { get; set; }
    }
}