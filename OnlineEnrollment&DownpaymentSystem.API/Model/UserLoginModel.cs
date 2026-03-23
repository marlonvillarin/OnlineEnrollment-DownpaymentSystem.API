namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class UserLoginModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
