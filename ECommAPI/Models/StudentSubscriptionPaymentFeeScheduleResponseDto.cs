namespace ECommAPI.Models
{
    public class StudentSubscriptionPaymentFeeScheduleResponseDto
    {
        public int FeeScheduleID { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
        public int SubscriptionTransactionID { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int ReceiptHeaderID { get; set; }
    }
}