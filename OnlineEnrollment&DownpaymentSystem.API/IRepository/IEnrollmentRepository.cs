using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IEnrollmentRepository
    {
        Task<ServiceResponse<EnrollmentModel>> CreateEnrollment(EnrollmentModel enrollment);
        Task<ServiceResponse<EnrollmentModel>> UpdateEnrollmentStatus(int enrollmentID, string status);
        Task<ServiceResponse<List<EnrollmentModel>>> GetEnrollmentsByStudent(int studentID);
        Task<ServiceResponse<List<EnrollmentModel>>> GetAllEnrollments();
    }
}