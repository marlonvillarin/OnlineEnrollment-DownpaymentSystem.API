using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface ISubjectRepository
    {
        Task<ServiceResponse<SubjectModel>> AddSubject(SubjectModel subject);
        Task<ServiceResponse<List<SubjectModel>>> GetAllSubjects();
        Task<ServiceResponse<List<SubjectModel>>> GetSubjectsByFilter(string course, int yearLevel, string semester);
        Task<ServiceResponse<SubjectModel>> UpdateSubject(SubjectModel subject);
        Task<ServiceResponse<string>> DeleteSubject(int subjectID);
    }
}
