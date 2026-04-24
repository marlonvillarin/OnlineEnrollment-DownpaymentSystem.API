namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class ValidationModel
    {
        public bool IsEligible { get; set; }
        public int MissingDocuments { get; set; }
        public int PendingPayment { get; set; }
        public int ApprovedPayment { get; set; }
        public string CurrentSemester { get; set; }
        public string CurrentSchoolYear { get; set; }
        public string NextSemester { get; set; }
        public string NextSchoolYear { get; set; }
        public string Remarks { get; set; }
    }
}