namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class StudentLoginModel
    {
        public int LoginID { get; set; }
        public int StudentID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
     
    }
    public class StudentCreateModel
    {
        public int StudentID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class StudentAccountListModel
    {
        public int LoginID { get; set; }
        public int StudentID { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string StudentNumber { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = "";
        public string StudentStatus { get; set; } = "";
    }

    public class StudentAccountModel
    {
        public int StudentID { get; set; }
        public string StudentNumber { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string Status { get; set; } = "";
    }
    public class StudentEligibilityModel
    {
        public bool IsEligible { get; set; }
        public string Message { get; set; } = "";
        public int StudentID { get; set; }
        public string StudentNumber { get; set; } = "";
        public string FullName { get; set; } = "";
        public bool HasLockGrades { get; set; }
        public bool HasLockClearance { get; set; }
        public bool HasLockPayment { get; set; }
        public decimal OutstandingBalance { get; set; }
        public int FailingGradesCount { get; set; }
        public int PendingDocumentsCount { get; set; }
    }
}