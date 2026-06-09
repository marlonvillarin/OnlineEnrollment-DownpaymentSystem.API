namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class StudentModel
    {
        public int StudentID { get; set; }
        public string? StudentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string StudentType { get; set; } 
        public string Status { get; set; } 
        public DateTime DateCreated { get; set; }
    }

    public class StudentDocumentModel
    {
        public int DocumentID { get; set; }
        public int StudentID { get; set; }
        public string DocumentType { get; set; }
        public string FilePath { get; set; }
        public bool IsApproved { get; set; }
        public DateTime UploadedDate { get; set; }
    }


    public class StudentDetailsModel
    {
        // Student Info
        public int StudentID { get; set; }
        public string StudentNumber { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string FullName => $"{LastName}, {FirstName} {MiddleName}".Trim();
        public string Email { get; set; } = "";
        public string ContactNumber { get; set; } = "";

        // Student Status
        public string StudentType { get; set; } = "";
        public string StudentStatus { get; set; } = "";

        // Enrollment Info
        public string Course { get; set; } = "";
        public string YearLevel { get; set; } = "";
        public string Semester { get; set; } = "";
        public string SchoolYear { get; set; } = "";
        public string EnrollmentStatus { get; set; } = "";

        // Document Status
        public string DocumentStatus { get; set; } = "";

        // Payment Status
        public string PaymentStatus { get; set; } = "";

        // Overall Status
        public string OverallStatus { get; set; } = "";

        // Pagination
        public int TotalCount { get; set; }
    }
    public class StudentsWithStatusResponse
    {
        public List<StudentDetailsModel> Students { get; set; } = new();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
    public class RegistrarEligibilityModel
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