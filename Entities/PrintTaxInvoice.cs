using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class PrintTaxInvoice
    {
        public Brand BrandDetails { get; set; } = new();
        public Centre CenterDetails { get; set; } = new();
        public Student StudentDetails { get; set; } = new();
        public string PlanName { get; set; }
        public string ProductName { get; set; }
        public string InvoiceNo { get; set; }
        public string BatchName { get; set; }
        public string PaymentMode { get; set; }
        public DateTime FeeScheduleDate { get; set; }
        public InvoiceDetailSummary InstalmentDetails { get; set; } = new();
    }
}
