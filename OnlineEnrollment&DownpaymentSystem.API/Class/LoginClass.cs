using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class LoginClass : ILoginRepository
    {
        public LoginClass() { }

        public async Task<ResponseModel<LoginModel>> GetLogin(string username, string password)
        {
            ResponseModel<LoginModel> sample = new ResponseModel<LoginModel>();
            return sample;
        }
    }
}
