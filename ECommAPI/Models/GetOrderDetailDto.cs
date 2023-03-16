namespace ECommAPI.Models
{
    public class GetOrderDetailDto
    {
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public string PlanImage { get; set; }
        public string FeeScheduleIDList { get; set; }
        public string ReceiptIDList { get; set; }
    }
}