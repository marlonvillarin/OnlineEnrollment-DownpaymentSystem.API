namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class StudentProfileModel
    {
        public int StudentID { get; set; }
        public string StudentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string StudentType { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
