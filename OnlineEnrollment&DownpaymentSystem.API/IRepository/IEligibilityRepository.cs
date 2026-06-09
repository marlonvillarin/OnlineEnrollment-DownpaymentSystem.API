using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IEligibilityRepository
    {
        Task<ServiceResponse<StudentEligibilityModel>> CheckEligibilityAsync(string studentNumber, string schoolYear, string semester);

        Task<ServiceResponse<List<RegistrarEligibilityModel>>> GetStudentsForRegistrarAsync(string schoolYear, string semester);
        Task<ServiceResponse<bool>> SaveRegistrarEligibilityAsync(RegistrarEligibilityModel eligibility);
    }
}
