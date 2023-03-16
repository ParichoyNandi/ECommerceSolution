using System;
using System.Collections.Generic;

namespace Entities
{
    public class InvoiceChildDetails
    {
        public int InvoiceChildDetailsId { get; set; }

        public int InvoiceChildHeaderId { get; set; }


        public int FeeComponentId { get; set; }

        public int InstallmentNo { get; set; }

        public DateTime DueDate { get; set; }

        public double AmountDue { get; set; }

        public double DiscountAmount { get; set; }

        public double PayableAmount { get; set; }

        public double CompanyShare { get; set; }

        public int Sequence { get; set; }

        public int DisplayFeeComponentId { get; set; }

        public int CourseFeePlanId { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsModified { get; set; }

        public bool IsDeleted { get; set; }

        public List<InvoiceTaxDetails> InvoiceTaxDetails { get; set; } = new();
    }
}