namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class TrackResponse
    {
        public bool StudentFound { get; set; }
        public StudentInfo? StudentInfo { get; set; }
        public EnrollmentInfo? Enrollment { get; set; }
        public DocumentInfo? Documents { get; set; }
        public PaymentInfo? Payment { get; set; }
        public OverallStatus Overall { get; set; } = new();
        public AccountInfo? Account { get; set; }
    }

    public class StudentInfo
    {
        public int StudentID { get; set; }
        public string? StudentNumber { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string StudentInfoStatus { get; set; } = "";
        public string FullName => $"{FirstName} {LastName}";
    }

    public class EnrollmentInfo
    {
        public string Course { get; set; } = "";
        public string SchoolYear { get; set; } = "";
        public string Semester { get; set; } = "";
        public string EnrollmentStatus { get; set; } = "";
    }

    public class DocumentInfo
    {
        public int TotalDocuments { get; set; }
        public int ApprovedDocuments { get; set; }
        public string Status => TotalDocuments > 0 && TotalDocuments == ApprovedDocuments ? "Approved" : "Pending";
    }

    public class PaymentInfo
    {
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = "";
        public string FormattedAmount => $"₱{Amount:F2}";
    }

    public class OverallStatus
    {
        public string StudentInfoStatus { get; set; } = "Pending";
        public bool AllDocumentsApproved { get; set; }
        public string EnrollmentStatus { get; set; } = "Pending";
        public string PaymentStatus { get; set; } = "Pending";
        public bool IsFullyApproved { get; set; }
    }

    public class AccountInfo
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

}
