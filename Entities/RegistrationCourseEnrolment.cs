using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class RegistrationCourseEnrolment
    {
        public int RegID { get; set; }
        public string CustomerID { get; set; }
        public List<CourseEnrolment> CourseList { get; set; } = new();
    }
}
