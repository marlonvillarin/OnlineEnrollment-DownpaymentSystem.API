using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IStudentRepository
    {
        Task<ServiceResponse<StudentProfileModel>> GetStudentProfile(int StudentID);
    }
}
