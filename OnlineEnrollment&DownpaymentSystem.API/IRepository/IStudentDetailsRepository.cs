using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IStudentDetailsRepository
    {
  
        Task<ServiceResponse<StudentModel>> ApproveStudentAsync(int studentId);
        Task<ServiceResponse<StudentModel>> RejectStudentAsync(int studentId);
        Task<ServiceResponse<StudentsWithStatusResponse>> GetAllStudentsWithStatusAsync(
              string searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10,
            string studentStatusFilter = null,
            string enrollmentStatusFilter = null,
            string paymentStatusFilter = null,
            string documentStatusFilter = null,
            string courseFilter = null,
            string yearLevelFilter = null,
            string studentTypeFilter = null,
            string overallStatusFilter = null);

    }
}
