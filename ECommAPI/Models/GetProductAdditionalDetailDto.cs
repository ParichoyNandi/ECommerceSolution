namespace ECommAPI.Models
{
    public class GetProductAdditionalDetailDto
    {
        public int ProductID { get; set; }
        public int CourseID { get; set; }
        public string TotalExams { get; set; }
        public string ExamsCovered { get; set; }
    }
}