using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreatePlanConfigMapDto
    {
        [Required]
        public int PlanID { get; set; }

        [Required]
        public int ConfigID { get; set; }
        public string ConfigValue { get; set; } = "";
        public string DisplayName { get; set; } = "";
    }
}
