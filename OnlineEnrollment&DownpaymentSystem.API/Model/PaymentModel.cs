namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class PaymentModel
    {
        public int PaymentID { get; set; }
        public int EnrollmentID { get; set; }
        public int StudentID { get; set; }
        public string StudentNumber { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FullName => $"{FirstName} {LastName}";
        public string YearLevel { get; set; } = "";
        public string Course { get; set; } = "";
        public string SchoolYear { get; set; } = "";
        public string Semester { get; set; } = "";
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } = "";
        public DateTime PaymentDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Remarks { get; set; }
    }

    public class PaymentRequest
    {
        public int EnrollmentID { get; set; }
        public decimal Amount { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Remarks { get; set; }
    }
}