using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateProductDto
    {
        [Required]
        public string ProductCode { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductShortDesc { get; set; }
        public string ProductLongDesc { get; set; }

        [Required]
        public int CourseID { get; set; }

        [Required]
        public int BrandID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Required]
        public bool IsPublished { get; set; } = false;

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        [Required]
        public string CreatedBy { get; set; } = "rice-group-admin";

        [Required]
        public string ProductImage { get; set; }

    }
}
