namespace ECommAPI.Models
{
    public class GetCourseEnrolmentDto
    {
        public int CourseID { get; set; }
        public bool CanBePurchased { get; set; } = false;
    }
}