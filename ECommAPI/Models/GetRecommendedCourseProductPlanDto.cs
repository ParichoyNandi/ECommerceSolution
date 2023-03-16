using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace ECommAPI.Models
{
    public class GetRecommendedCourseProductPlanDto
    {
        public GetCourseDto Course { get; set; } = new();
        public List<GetProductPlanDto> ProductPlanLists { get; set; } = new List<GetProductPlanDto>();
    }
}
