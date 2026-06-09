namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class StudentClearanceModel
    {
        public int StudentID { get; set; }
        public string StudentNumber { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Course { get; set; } = "";
        public int YearLevel { get; set; }
        public string Semester { get; set; } = "";
        public string SchoolYear { get; set; } = "";

    
        public bool IsGradeCleared { get; set; }
        public bool IsClearanceCleared { get; set; }
        public bool IsPaymentCleared { get; set; }

        public string? GradeMessage { get; set; }
        public string? ClearanceMessage { get; set; }
        public string? PaymentMessage { get; set; }

      
        public bool IsFullyCleared => IsGradeCleared && IsClearanceCleared && IsPaymentCleared;
    }
}
