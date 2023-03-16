using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommPortal.Models
{
    public class CreateDiscountMappingsViewModel:ResponseModel
    {

        public int MappedDiscountId { get; set; }
        public List<Course> CourseList { get; set; } = new();
        public List<DiscountScheme> DiscountList { get; set; } = new();

        public List<Centre> CenterList { get; set; } = new();

        public List<CourseCenter> CoursecenterList { get; set; } = new();

      
    }


    public class CourseCenter
    {
        public List<int> SelectedCourseList { get; set; }
        public int Center { get; set; }

    }
}
