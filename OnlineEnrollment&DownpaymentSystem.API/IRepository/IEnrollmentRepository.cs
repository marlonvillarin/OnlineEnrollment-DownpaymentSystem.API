using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IEnrollmentRepository
    {
        Task<ServiceResponse<object>> EnrollStudent(ResponseModel student);
    }
}
