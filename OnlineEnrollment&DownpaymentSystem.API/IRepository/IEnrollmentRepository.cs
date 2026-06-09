using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IEnrollmentRepository
    {

        Task<ServiceResponse<EnrollmentStudent>> CreateEnrollment(EnrollmentStudent enrollment);

        Task<ServiceResponse<EnrollmentStudent>> UpdateEnrollment(EnrollmentStudent enrollment);
        Task<ServiceResponse<EnrollmentStudent>> UpdateEnrollmentStatus(int enrollmentID, string status);


        Task<ServiceResponse<EnrollmentStudent>> ApproveEnrollment(int enrollmentID);
        Task<ServiceResponse<EnrollmentStudent>> RejectEnrollment(int enrollmentID);
        Task<ServiceResponse<EnrollmentStudent>> CompleteEnrollment(int enrollmentID);
        Task<ServiceResponse<List<EnrollmentModel>>> GetAllEnrollmentsWithStudentInfoAsync();

        
        Task<ServiceResponse<List<EnrollmentStudent>>> GetPendingEnrollments();
        Task<ServiceResponse<List<EnrollmentStudent>>> GetEnrollmentsByStudent(int studentID);




        Task<ServiceResponse<List<EnrollmentModel>>> GetAdminPendingEnrollmentsAsync();
        Task<ServiceResponse<List<EnrollmentModel>>> GetApprovedEnrollmentsAsync();
        Task<ServiceResponse<EnrollmentModel>> AdminApproveEnrollmentAsync(int enrollmentId);
        Task<ServiceResponse<EnrollmentModel>> AdminRejectEnrollmentAsync(int enrollmentId);
    }
}