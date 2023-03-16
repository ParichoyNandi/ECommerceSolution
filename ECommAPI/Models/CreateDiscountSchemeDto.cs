using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateDiscountSchemeDto
    {
        [Required]
        public string DiscountSchemeName { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        [Required]
        public string BrandList { get; set; }

        [Required]
        public List<CreateDiscountSchemeDetailDto> DiscountSchemeDetails { get; set; } = new();

        [Required]
        public string CreatedBy { get; set; }
        //public List<CreateDiscountCenterCourseMap> MyProperty { get; set; }
    }
}
