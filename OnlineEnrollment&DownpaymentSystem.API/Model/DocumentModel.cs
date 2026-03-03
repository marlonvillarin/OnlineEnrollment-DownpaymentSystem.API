namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class DocumentModel
    {
        public int DocumentID { get; set; }
        public int StudentID { get; set; }
        public string DocumentType { get; set; } = "";
        public string FilePath { get; set; } = "";
        public bool IsApproved { get; set; } = false;
        public DateTime UploadedDate { get; set; }
    }
}
