using System;

namespace ECommAPI.Models
{
    public class GetStudentSubscriptionDueDto
    {
        public int InvoiceDetailID { get; set; }
        public string InvoiceNo { get; set; }
        public int InstallmentNo { get; set; }
        public DateTime InstallmentDate { get; set; }
        public int Sequence { get; set; }
        public int FeeComponentID { get; set; }
        public string FeeComponentName { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal PayableTax { get; set; }
        public decimal TotalPayableAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal TaxPaid { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal DueBaseAmount
        {
            get
            {
                return PayableAmount - AmountPaid;
            }
        }
        public decimal DueTaxAmount
        {
            get
            {
                return PayableTax - TaxPaid;
            }
        }
        public decimal DueAmount { get; set; }
        public string DueStatus
        {
            get
            {
                if (DueAmount > 0 && DueAmount < TotalPayableAmount)
                    return "Partial Paid";
                else if (DueAmount > 0 && DueAmount == TotalPayableAmount)
                    return "Full Due";
                else
                    return "Full Paid";
            }
        }
    }
}