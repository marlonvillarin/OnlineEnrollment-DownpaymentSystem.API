using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IPaymentRepository
    {
        //  CREATE
        Task<ServiceResponse<PaymentModel>> CreatePayment(int enrollmentID, decimal amount);

        //  INTERNAL
        Task<ServiceResponse<PaymentModel>> UpdatePaymentStatus(int paymentID, string status);

        //  CASHIER ACTIONS
        Task<ServiceResponse<PaymentModel>> ApprovePayment(int paymentID);
        Task<ServiceResponse<PaymentModel>> RejectPayment(int paymentID);

        //  GET DATA
        Task<ServiceResponse<List<PaymentModel>>> GetAllPayments();
        Task<ServiceResponse<List<PaymentModel>>> GetPendingPayments();
        Task<ServiceResponse<List<PaymentModel>>> GetPaymentsByEnrollment(int enrollmentID);
        Task<ServiceResponse<PaymentModel>> GetPaymentByID(int paymentID);
    }
}