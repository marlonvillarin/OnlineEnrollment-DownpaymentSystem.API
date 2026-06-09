using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IPaymentRepository
    {
        Task<ServiceResponse<PaymentModel>> CreatePayment(
                int enrollmentID,
                decimal amount,
                string? referenceNumber = null,
                string? paymentMethod = null,
                string? remarks = null);

        Task<ServiceResponse<List<PaymentModel>>> GetPendingPaymentsAsync(string searchTerm = null);
        Task<ServiceResponse<PaymentModel>> ApprovePaymentAsync(int paymentId);
        Task<ServiceResponse<PaymentModel>> RejectPaymentAsync(int paymentId);
        Task<ServiceResponse<List<PaymentModel>>> GetApprovedPaymentsAsync(string searchTerm = null);
        Task<ServiceResponse<List<PaymentModel>>> GetAllPaymentsAsync(string searchTerm = null);

    }
}