using System;
using System.Collections.Generic;

namespace ECommAPI.Models
{
    public class GetStudentSubscriptionFeeScheduleDto
    {
        public int FeeScheduleID { get; set; }
        public DateTime FeeScheduleDate { get; set; }
        public string CourseName { get; set; }
        public List<GetStudentSubscriptionDueDto> DueDetails { get; set; } = new();
    }
}