namespace Entities
{
    public class DiscountSchemeDetail
    {
        public int DiscountSchemeDetailID { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public int IsApplicableOn { get; set; }
        public string ApplicableOn 
        {
            get
            {
                if (IsApplicableOn == 1)
                    return "Lumpsum";
                else if (IsApplicableOn == 2)
                    return "Instalment";
                else
                    return "";
            }
        }
        public string FromInstalment { get; set; }
        public string FeeComponentID { get; set; }
    }
}