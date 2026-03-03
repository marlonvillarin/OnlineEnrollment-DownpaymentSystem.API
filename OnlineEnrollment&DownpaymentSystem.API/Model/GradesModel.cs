namespace OnlineEnrollment_DownpaymentSystem.API.Model
{
    public class GradesModel
    {
        public int GradeID { get; set; }
        public int StudentID { get; set; }
        public string SubjectName { get; set; }
        public string SchoolYear { get; set; }
        public string Semester { get; set; }
        public decimal? Grade { get; set; }
        public string? Remarks { get; set; }
    }
}