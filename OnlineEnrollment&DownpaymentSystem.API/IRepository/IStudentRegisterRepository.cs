using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IStudentRegisterRepository
    {
        Task<ServiceResponse<object>> RegisterAdmin(RegisterAccountModel model);
       
    }
}
