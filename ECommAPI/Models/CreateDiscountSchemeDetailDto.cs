namespace ECommAPI.Models
{
    public class CreateDiscountSchemeDetailDto
    {
        public decimal DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public int IsApplicableOn { get; set; }
        public string FromInstalment { get; set; }
        public string FeeComponentID { get; set; }
    }
}