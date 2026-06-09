using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface ILoginRepository
    {
        Task<ServiceResponse<StudentLoginModel>> CreateLogin(int studentID, string username, string password);
        Task<ServiceResponse<StudentLoginModel>> Authenticate(string username, string password);


        Task<ServiceResponse<StudentAccountModel>> GetStudentByIdAsync(int studentId);
        Task<ServiceResponse<List<StudentAccountListModel>>> GetAllStudentAccountsAsync(string searchTerm = null);
        Task<bool> AccountExistsAsync(int studentId);
    }
}