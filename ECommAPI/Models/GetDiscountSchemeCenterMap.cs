using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetDiscountSchemeCenterMap
    {
        public int DiscountCenterDetailID { get; set; }
        public int CenterID { get; set; }
        public List<GetDiscountSchemeCenterCourseMap> CenterCourseMaps { get; set; } = new();
    }
}
