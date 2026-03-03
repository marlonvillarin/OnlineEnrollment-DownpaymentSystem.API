namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class StudentLoginModel
    {
        public int LoginID { get; set; }
        public int StudentID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}