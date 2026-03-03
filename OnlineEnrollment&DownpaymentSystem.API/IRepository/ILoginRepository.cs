using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface ILoginRepository
    {
        Task<ServiceResponse<StudentLoginModel>> CreateLogin(int studentID, string username, string password);
        Task<ServiceResponse<StudentLoginModel>> Authenticate(string username, string password);
    }
}