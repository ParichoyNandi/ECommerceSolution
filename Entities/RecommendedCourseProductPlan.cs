using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class RecommendedCourseProductPlan
    {
        public Course Course { get; set; } = new();
        public List<ProductPlan> ProductPlanLists { get; set; } = new List<ProductPlan>();
       
    }
}
