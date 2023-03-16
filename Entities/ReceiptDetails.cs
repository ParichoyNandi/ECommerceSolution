using System;
using System.Collections.Generic;

namespace Entities
{
    public class ReceiptDetails
    {
        public int ReceiptComponentDetailId { get; set; }

        public int InvoiceDetailId { get; set; }

        public double AmountPaid { get; set; }

        public double ReceiptDetailsRff { get; set; }

        public InvoiceChildDetails InvoiceChildDetails { get; set; }

        public List<ReceiptTaxDetails> ReceiptTaxDetails { get; set; } = new();

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsModified { get; set; }

        public bool IsDeleted { get; set; }
    }
}