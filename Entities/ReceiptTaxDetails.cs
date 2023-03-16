using System;

namespace Entities
{
    public class ReceiptTaxDetails
    {
        public int TaxId { get; set; }

        public double TaxPaid { get; set; }

        public double ReceiptTaxRff { get; set; }

        public int ReceiptcomponentDetailId { get; set; }

        public int InvoiceDetailId { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool IsModified { get; set; }

        public bool IsDeleted { get; set; }
    }
}