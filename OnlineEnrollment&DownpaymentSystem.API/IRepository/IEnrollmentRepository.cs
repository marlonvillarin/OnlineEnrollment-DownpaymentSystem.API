using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IEnrollmentRepository
    {
      
        Task<ServiceResponse<EnrollmentModel>> CreateEnrollment(EnrollmentModel enrollment);

      
        Task<ServiceResponse<EnrollmentModel>> UpdateEnrollmentStatus(int enrollmentID, string status);

     
        Task<ServiceResponse<EnrollmentModel>> ApproveEnrollment(int enrollmentID);
        Task<ServiceResponse<EnrollmentModel>> RejectEnrollment(int enrollmentID);
        Task<ServiceResponse<EnrollmentModel>> CompleteEnrollment(int enrollmentID);

       
        Task<ServiceResponse<List<EnrollmentModel>>> GetAllEnrollments();
        Task<ServiceResponse<List<EnrollmentModel>>> GetPendingEnrollments();
        Task<ServiceResponse<List<EnrollmentModel>>> GetEnrollmentsByStudent(int studentID);
    }
}