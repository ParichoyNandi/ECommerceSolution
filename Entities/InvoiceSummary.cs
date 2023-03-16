using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class InvoiceSummary
    {
        public string CourseName { get; set; }
        public string SACCode { get; set; }
        public decimal CourseAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal OldTax { get; set; }
        public decimal SGSTTax { get; set; }
        public decimal CGSTTax { get; set; }
        public decimal IGSTTax { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public List<InvoiceDetailSummary> InstalmentDetails { get; set; } = new();
    }
}
