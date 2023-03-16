namespace Entities
{
    public class OrderDetail
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public string PlanImage { get; set; }
        public string FeeScheduleIDList { get; set; }
        public string ReceiptIDList { get; set; }
    }
}