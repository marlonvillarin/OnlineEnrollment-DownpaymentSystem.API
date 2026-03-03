namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class NotificationModel
    {
        public int NotificationID { get; set; }
        public int StudentID { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}