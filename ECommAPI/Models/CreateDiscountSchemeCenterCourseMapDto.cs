using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class CreateDiscountSchemeCenterCourseMapDto
    {
        public int DiscountSchemeID { get; set; }
        public int CenterID { get; set; }
        public int CourseID { get; set; }
        public string CreatedBy { get; set; }
    }
}
