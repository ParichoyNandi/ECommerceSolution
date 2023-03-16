using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommAPI.Models
{
    public class GetStudentPayOutDto
    {
        public string CustomerID { get; set; }
        public List<GetStudentPayOutFeeScheduleDto> FeeSchedules { get; set; } = new();
    }
}
