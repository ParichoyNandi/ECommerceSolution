namespace ECommAPI.Models
{
    public class StudentPayOutPaymentFeeScheduleResponseDto
    {
        public int FeeScheduleID { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int ReceiptHeaderID { get; set; }
    }
}