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
        public string StudentType { get; set; } // New / Old
        public string Status { get; set; } // Pending / Approved
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
}