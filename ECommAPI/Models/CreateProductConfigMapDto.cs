using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateProductConfigMapDto
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public int ConfigID { get; set; }
        public string ConfigValue { get; set; } = "";
        public string ConfigDisplayName { get; set; } = "";
        public int SubHeaderID { get; set; } = 0;
        public string SubHeaderDisplayName { get; set; } = null;
        public int HeaderID { get; set; } = 0;
        public string HeaderDisplayName { get; set; } = null;
    }
}
