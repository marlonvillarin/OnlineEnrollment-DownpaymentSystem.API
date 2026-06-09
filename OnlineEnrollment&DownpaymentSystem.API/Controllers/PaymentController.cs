using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            var response = await _paymentRepository.CreatePayment(
                request.EnrollmentID,
                request.Amount,
                request.ReferenceNumber,
                request.PaymentMethod,
                request.Remarks
            );
            return StatusCode(response.Status, response);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingPayments([FromQuery] string search = null)
        {
            var response = await _paymentRepository.GetPendingPaymentsAsync(search);
            return StatusCode(response.Status, response);
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApprovePayment(int id)
        {
            var response = await _paymentRepository.ApprovePaymentAsync(id);
            return StatusCode(response.Status, response);
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> RejectPayment(int id)
        {
            var response = await _paymentRepository.RejectPaymentAsync(id);
            return StatusCode(response.Status, response);
        }

        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedPayments([FromQuery] string search = null)
        {
            var response = await _paymentRepository.GetApprovedPaymentsAsync(search);
            return StatusCode(response.Status, response);
        }

      
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPayments([FromQuery] string search = null)
        {
            var response = await _paymentRepository.GetAllPaymentsAsync(search);
            return StatusCode(response.Status, response);
        }

    }
}