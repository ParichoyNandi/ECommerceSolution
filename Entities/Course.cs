using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }

        public int CourseLanguageID { get; set; }//susmita
        public string CourseLangaugeName { get; set; }//susmita
    }
}
