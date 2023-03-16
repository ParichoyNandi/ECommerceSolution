using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Registration
    {
        public int RegID { get; set; }
        public string CustomerID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; } = "";
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public DateTime DoB { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pincode { get; set; }
        public string HighestEducationQualification { get; set; }
        public int StatusID { get; set; }
        public string RegistrationSource { get; set; }
        public string GuardianName { get; set; }
        public string GuardianMobileNo { get; set; }
        public string SecondLanguage { get; set; }
        public string SocialCategory { get; set; }
        public List<RegistrationCourse> RegCourseList { get; set; } = new();

        public int LanguageID { get; set; }//susmita: 08-08-2022

        public string LanguageName { get; set; }//susmita:08-08-2022
    }
}
