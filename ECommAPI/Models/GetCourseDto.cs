using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetCourseDto
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }

        public int CourseLanguageID { get; set; }//susmita
        public string CourseLangaugeName { get; set; }//susmita
    }
}
