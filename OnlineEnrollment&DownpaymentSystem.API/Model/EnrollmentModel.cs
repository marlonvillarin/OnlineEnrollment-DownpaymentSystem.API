namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class EnrollmentModel
    {
        public int EnrollmentID { get; set; }
        public int StudentID { get; set; }
        public string Course { get; set; }
        public int YearLevel { get; set; }
        public string SchoolYear { get; set; }
        public string Semester { get; set; }
        public string EnrollmentStatus { get; set; } 

    }
}