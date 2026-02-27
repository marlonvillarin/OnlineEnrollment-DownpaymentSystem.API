using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IEnrollmentRepository
    {
        Task<ServiceResponse<List<EnrollmentModel>>> GetEnrollment(int studentId);
    }
}
