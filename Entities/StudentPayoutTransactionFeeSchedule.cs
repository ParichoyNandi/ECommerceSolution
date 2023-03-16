namespace Entities
{
    public class StudentPayoutTransactionFeeSchedule
    {
        public int BrandID { get; set; }
        public string StudentID { get; set; }
        public int StudentDetailID { get; set; }
        public int CenterID { get; set; }
        public int FeeScheduleID { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int ReceiptHeaderID { get; set; }
    }
}