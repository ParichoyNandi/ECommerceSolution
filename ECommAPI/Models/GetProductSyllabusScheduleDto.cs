using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetProductSyllabusScheduleDto
    {
        public List<GetProductAdditionalDetailDto> AdditionalData { get; set; } = new();
        public List<Syllabus> Syllabuses { get; set; } = new();
        public List<Schedule> Schedules { get; set; } = new();
    }
}
