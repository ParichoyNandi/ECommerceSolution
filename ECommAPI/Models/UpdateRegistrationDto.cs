using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class UpdateRegistrationDto
    {
        [Required]
        public string CustomerID { get; set; }

        //[Required]
        public string GuardianName { get; set; }

        //[Required]
        public string GuardianMobileNo { get; set; }

        //[Required]
        public string SecondLanguage { get; set; }

        //[Required]
        public string SocialCategory { get; set; }
    }
}
