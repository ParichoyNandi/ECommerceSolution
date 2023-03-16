using System;

namespace Entities
{
    public class ProductTransaction
    {
        public int TransactionProductDetailID { get; set; }
        public int ProductID { get; set; }
        public int BrandID { get; set; }
        public int CenterID { get; set; }
        public int FeePlanID { get; set; }
        public int PaymentModeID { get; set; }
        public int BatchID { get; set; }
        public int CourseID { get; set; }
        public int DeliveryPatternID { get; set; }
        public DateTime BatchStartDate { get; set; }
        public int DiscountSchemeID { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal TaxPaid { get; set; }
        public bool CanBeProcessed { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
        public int FeeScheduleID { get; set; }
        public DateTime FeeScheduleDate { get; set; }
        public string StudentID { get; set; }
        public int ReceiptHeaderID { get; set; }
        public string ReceiptDate { get; set; }
        public ProductSubscription SubscriptionDetails { get; set; } = new();
    }
}