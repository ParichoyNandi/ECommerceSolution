using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class StudentPayOutFeeSchedule
    {
        public string StudentID { get; set; }
        public string TransactionNo { get; set; }
        public int PlanID { get; set; }
        public int ProductID { get; set; }
        public int FeeScheduleID { get; set; }
        public DateTime FeeScheduleDate { get; set; }
        public string CourseName { get; set; }
        public List<StudentSubscriptionDue> DueDetails { get; set; } = new();
    }
}
