using System;
using System.Collections.Generic;

namespace Entities
{
    public class StudentSubscriptionFeeSchedule
    {
        public int FeeScheduleID { get; set; }
        public DateTime FeeScheduleDate { get; set; }
        public string CourseName { get; set; }
        public List<StudentSubscriptionDue> DueDetails { get; set; } = new();
    }
}