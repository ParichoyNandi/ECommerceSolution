using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace ECommAPI.Models
{
    public class GetProductPlanDto
    {
        public int ProductID { get; set; }
        public int CourseID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ShortDesc { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string ProductImage { get; set; }

        public List<PlanDto> PlanLists { get; set; } = new List<PlanDto>();

    }
}
