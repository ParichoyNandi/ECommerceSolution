using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PrintFeeSchedule
    {
        public Brand BrandDetails { get; set; } = new();
        public Centre CenterDetails { get; set; } = new();
        public Student StudentDetails { get; set; } = new();
        public string PlanName { get; set; }
        public string ProductName { get; set; }
        public string InvoiceNo { get; set; }
        public string BatchName { get; set; }
        public DateTime FeeScheduleDate { get; set; }
        public InvoiceSummary FeeScheduleSummary { get; set; } = new();
    }
}
