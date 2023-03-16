using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateRegistrationDto
    {
        [Required]
        public string CustomerID { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress,ErrorMessage ="Not a valid email address")]
        public string EmailID { get; set; }

        [Required]
        public string MobileNo { get; set; }

        [Required]
        public string HighestEducationQualification { get; set; }

        [Required]
        public string RegistrationSource { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DoB { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Pincode { get; set; }

        [Required]
        public List<CreateRegistrationCourseDto> RegCourseList { get; set; } = new();

        [Required]
        public int LanguageID { get; set; }//susmita: 08-08-2022


        public string LanguageName { get; set; } = null;   //susmita:08-08-2022

    }
}
