namespace Entities
{
    public class InvoiceTaxDetails
    {
        public int TaxId { get; set; }

        public int CGSTSGST { get; set; }

        public int InvoiceChildDetailId { get; set; }

        public double TaxValue { get; set; }

        
        public string TaxCode { get; set; }

        public string TaxDescription { get; set; }
    }
}