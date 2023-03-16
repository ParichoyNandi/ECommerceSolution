using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class FeeSchedule
    {
        public int InvoiceId { get; set; }

        public string Remarks { get; set; }

        public string InvoiceNo { get; set; }

        public int StudentDetailID { get; set; }

        public int CenterID { get; set; }

        public double InvoiceAmount { get; set; }

        public double TotalTaxAmount { get; set; }

        public double DiscountAmount { get; set; }
        public DateTime InvoiceDate { get; set; }

        public int DiscountSchemeId { get; set; }

        public int DiscountAppliedAt { get; set; }

        public int CouponDiscount { get; set; }

        public string CouponNumber { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsModified { get; set; }

        public bool IsDeleted { get; set; }

        public List<InvoiceChild> InvoiceChild { get; set; } = new();

        public int InvoiceStatus { get; set; }

        public int LinkedInvoiceHeaderId { get; set; }

        public string LinkedInvoiceNo { get; set; }

        public int TransferOutInvoiceID { get; set; }
        public string CancellationReason { get; set; }
        public int ParentInvoiceID { get; set; }
        public int ParentInvoiceNumber { get; set; }
    }
}
