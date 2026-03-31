using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IPaymentRepository
    {
        Task<ServiceResponse<PaymentModel>> CreatePayment(int enrollmentID, decimal amount);
        Task<ServiceResponse<PaymentModel>> UpdatePaymentStatus(int paymentID, string status);
        Task<ServiceResponse<List<PaymentModel>>> GetPaymentsByEnrollment(int enrollmentID);
        Task<ServiceResponse<PaymentModel>> GetPaymentByID(int paymentID);
        Task<ServiceResponse<List<PaymentModel>>> GetAllPayments();
    }
}