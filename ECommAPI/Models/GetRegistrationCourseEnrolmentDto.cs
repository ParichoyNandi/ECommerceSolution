using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetRegistrationCourseEnrolmentDto
    {
        public int RegID { get; set; }
        public string CustomerID { get; set; }
        public List<GetCourseEnrolmentDto> CourseList { get; set; } = new();
    }
}
