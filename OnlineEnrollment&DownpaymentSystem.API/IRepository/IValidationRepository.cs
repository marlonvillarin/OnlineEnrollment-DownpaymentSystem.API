using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IValidationRepository
    {
        Task<ServiceResponse<ValidationModel>> ValidateStudentForEnrollment(int studentID);
    }
}