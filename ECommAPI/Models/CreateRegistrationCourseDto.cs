using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateRegistrationCourseDto
    {
        [Required]
        public string CourseName { get; set; }
    }
}
