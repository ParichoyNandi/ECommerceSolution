using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ECommPortal.Services
{
    public class LMSSMSSyncDashboardProperty
    {
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string StudentBatch { get; set; }
        public int BatchID { get; set; }
        public int CourseID { get; set; }
        public string Course { get; set; }
        public bool ActionStatus { get; set; }
        public int Attempt { get; set; }
        public string StatusID { get; set; }

        public String GuardianName { get; set; }
        public long ContactNo { get; set; }
        public DateTime SampleDate { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string createdBy { get; set; } = "rice-group-admin";
      
    }
}
