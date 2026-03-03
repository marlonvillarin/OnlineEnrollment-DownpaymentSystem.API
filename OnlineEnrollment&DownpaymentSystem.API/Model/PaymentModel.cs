namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class PaymentModel
    {
        public int PaymentID { get; set; }
        public int EnrollmentID { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}