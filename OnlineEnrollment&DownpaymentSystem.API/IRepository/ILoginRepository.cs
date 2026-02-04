using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface ILoginRepository
    {
        public Task<ResponseModel<LoginModel>> GetLogin(string username, string password);
        //public object GetLogin(string username, string password);
    }
}
