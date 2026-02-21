using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface ILoginRepository
    {
         Task<ServiceResponse<object>> GetLogin(string username, string password);
        //public object GetLogin(string username, string password);
    }
}
