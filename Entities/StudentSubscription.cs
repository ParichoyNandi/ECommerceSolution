using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class StudentSubscription
    {
        public string CustomerID { get; set; }
        public string StudentID { get; set; }
        public int SubscriptionID { get; set; }
        public string AuthKey { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public List<StudentSubscriptionFeeSchedule> FeeSchedules { get; set; } = new();
    }
}
