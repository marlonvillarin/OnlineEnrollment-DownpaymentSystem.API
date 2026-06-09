namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class EnrollmentStudent
    {
        public int EnrollmentID { get; set; }
        public int StudentID { get; set; }
        public string Course { get; set; }
        public int YearLevel { get; set; }
        public string SchoolYear { get; set; }
        public string Semester { get; set; }
        public string EnrollmentStatus { get; set; } 

    }

    public class EnrollmentModel
    {
        public int EnrollmentID { get; set; }
        public int StudentID { get; set; }

        public string StudentNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

      
        public string FullName => $"{LastName}, {FirstName}";

        
        public string Course { get; set; } = string.Empty;
        public string YearLevel { get; set; } = string.Empty;
        public string SchoolYear { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public string EnrollmentStatus { get; set; } = string.Empty;
    }
}