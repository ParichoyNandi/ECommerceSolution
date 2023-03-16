using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class DiscountSchemeCenterMap
    {
        public int DiscountCenterDetailID { get; set; }
        public int CenterID { get; set; }
        public List<DiscountSchemeCenterCourseMap> CenterCourseMaps { get; set; } = new();
    }
}
