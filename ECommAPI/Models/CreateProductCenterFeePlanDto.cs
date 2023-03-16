using System;
using System.ComponentModel.DataAnnotations;

namespace ECommAPI.Models
{
    public class CreateProductCenterFeePlanDto
    {
        [Required]
        public int CenterID { get; set; }

        [Required]
        public int FeePlanID { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }
    }
}