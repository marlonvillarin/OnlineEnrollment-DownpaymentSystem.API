namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class ValidationModel
    {
        public bool HasIncompleteGrades { get; set; }
        public bool HasPendingBalance { get; set; }
        public bool HasPendingEnrollment { get; set; }
        public bool CanEnroll { get; set; }
    }
}