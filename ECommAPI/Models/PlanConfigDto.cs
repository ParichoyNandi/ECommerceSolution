namespace ECommAPI.Models
{
    public class PlanConfigDto
    {
        public int PlanConfigID { get; set; }
        public int ConfigID { get; set; }
        public string ConfigCode { get; set; }
        public string DisplayName { get; set; }
        public string ConfigValue { get; set; }
    }
}