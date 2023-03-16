namespace Entities
{
    public class CourseEnrolment
    {
        public int CourseID { get; set; }
        public bool CanBePurchased { get; set; } = false;
    }
}