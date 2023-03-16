using System;
using System.Collections.Generic;

namespace ECommAPI.Models
{
    public class GetStudentPayOutFeeScheduleDto
    {
        public string StudentID { get; set; }
        public string TransactionNo { get; set; }
        public int PlanID { get; set; }
        public int ProductID { get; set; }
        public int FeeScheduleID { get; set; }
        public DateTime FeeScheduleDate { get; set; }
        public string CourseName { get; set; }
        public List<GetStudentSubscriptionDueDto> DueDetails { get; set; } = new();
    }
}