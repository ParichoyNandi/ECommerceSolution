using System;

namespace Entities
{
    public class InvoiceDetailSummary
    {
        public int InvoiceHeaderID { get; set; }
        public int InstalmentNo { get; set; }
        public DateTime InstalmentDate { get; set; }
        public decimal AmountDue { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceTYpe { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal TotalAmount { get; set; }
    }
}