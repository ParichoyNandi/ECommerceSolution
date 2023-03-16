namespace ECommAPI.Models
{
    public class CreateSubscriptionFeeSchedulePaymentDto
    {
        public int FeeScheduleID { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
    }
}