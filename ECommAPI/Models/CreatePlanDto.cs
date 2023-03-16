using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreatePlanDto
    {
        [Required]
        public string PlanCode { get; set; }

        [Required]
        public string PlanName { get; set; }
        public string PlanDesc { get; set; }

        [Required]
        public string BrandIDList { get; set; }

        [Required]
        public string PlanImage { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        [Required]
        public int PlanLanguageID { get; set; } = 2;//susmita

        [Required]
        public string PlanLanguageName { get; set; } = "Bengali & English";//susmita

        [Required]
        public string CreatedBy { get; set; }
    }
}
