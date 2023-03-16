namespace Entities
{
    public class ProductPayment
    {
        
        public int ProductID { get; set; }
        public int BatchID { get; set; }
        public int ProductCentreID { get; set; }
        public int PaymentMode { get; set; }
        public string CouponCode { get; set; }
        public int ProductFeePlanID { get; set; }
        public decimal ProspectusAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PaidTax { get; set; }
        public ProductSubscription SubscriptionDetails { get; set; } = new();
    }
}