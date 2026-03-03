using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IGradesRepository
    {
        Task<ServiceResponse<GradesModel>> AddGrade(int studentID, string subjectName, string schoolYear, string semester, decimal? grade, string remarks);
        Task<ServiceResponse<GradesModel>> UpdateGrade(int gradeID, decimal? grade, string remarks);
        Task<ServiceResponse<List<GradesModel>>> GetGradesByStudent(int studentID);
        Task<ServiceResponse<GradesModel>> GetGradeByID(int gradeID);
    }
}