using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateProductMappingDto
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public List<CreateProductCenterFeePlanDto> CenterFeePlanDetails { get; set; } = new();

        [Required]
        public string ExamCategoryList { get; set; }

        [Required]
        public string CreatedBy { get; set; }
    }
}
