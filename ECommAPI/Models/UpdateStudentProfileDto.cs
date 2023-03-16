using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class UpdateStudentProfileDto
    {
        [Required]
        public string CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HighestEducationQualification { get; set; }
        public string Gender { get; set; }
        [Required]
        public DateTime DoB { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string GuardianName { get; set; }
        public string GuardianMobileNo { get; set; }
        public string SecondLanguage { get; set; }
        public string SocialCategory { get; set; }
    }
}
