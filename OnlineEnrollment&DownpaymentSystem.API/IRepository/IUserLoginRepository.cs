using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IUserLoginRepository
    {
        Task<ServiceResponse<UserLoginModel>> CreateLogin(string username, string password, string role);
        Task<ServiceResponse<UserLoginModel>> Authenticate(string username, string password);
    }
}
