using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateCouponDto
    {
        [Required]
        public string CouponCode { get; set; }

        [Required]
        public string CouponName { get; set; }
        public string CouponDesc { get; set; }
        //public int BrandID { get; set; }

        [Required]
        public int DiscountID { get; set; }

        [Required]
        public int CouponCategoryID { get; set; }

        [Required]
        public int CouponTypeID { get; set; }

        [Required]
        public int CouponCount { get; set; }

        public int PerStudentCount { get; set; } = 1;
        public decimal GreaterThanAmount { get; set; }
        public string CustomerCode { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        [Required]
        public string BrandList { get; set; }
        public bool IsPrivate { get; set; } = false;
    }
}
