using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateConfigDto
    {
        [Required]
        public string ConfigCode { get; set; }

        [Required]
        public string ConfigName { get; set; }
        public string ConfigDefaultValue { get; set; }
    }
}
