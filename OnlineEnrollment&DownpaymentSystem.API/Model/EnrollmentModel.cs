namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class EnrollmentModel
    {
        public int EnrollmentID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public string SchoolYear { get; set; }
        public string Semester { get; set; }
        public string EnrollmentStatus { get; set; }
    }
}
