using System;

namespace Entities
{
    public class StudentSubscriptionDue
    {
        public int InvoiceDetailID { get; set; }
        public string InvoiceNo { get; set; }
        public int InstallmentNo { get; set; }
        public DateTime InstallmentDate { get; set; }
        public int Sequence { get; set; }
        public int FeeComponentID { get; set; }
        public string FeeComponentName { get; set; }
        public decimal InitialAmountDue { get; set; }
        public decimal InitialTaxDue { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal TaxPaid { get; set; }
        public decimal InitialPayableAmount
        {
            get
            {
                return InitialAmountDue + InitialTaxDue;
            }
        }
        public decimal TotalPaidAmount
        {
            get
            {
                return AmountPaid + TaxPaid;
            }
        }
        public decimal CreditNoteAmount { get; set; }
        public decimal CreditNoteTax { get; set; }
        public decimal FinalPayableAmount
        {
            get
            {
                return InitialAmountDue - CreditNoteAmount;
            }
        }
        public decimal FinalPayableTax
        {
            get
            {
                return InitialTaxDue - CreditNoteTax;
            }
        }

        public decimal TotalPayableAmount
        {
            get
            {
                return InitialAmountDue + InitialTaxDue - CreditNoteAmount - CreditNoteTax;
            }
        }
        public decimal DueAmount
        {
            get
            {
                return TotalPayableAmount - TotalPaidAmount;
            }
        }
    }
}