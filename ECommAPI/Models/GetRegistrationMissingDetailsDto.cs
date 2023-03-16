using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetRegistrationMissingDetailsDto
    {
        public string CustomerID { get; set; }
        public string GuardianName { get; set; }
        public string GuardianMobileNo { get; set; }
        public string SecondLanguage { get; set; }
        public string SocialCategory { get; set; }
    }
}
