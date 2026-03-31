using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment(int enrollmentID, decimal amount)
        {
            var response = await _paymentRepository.CreatePayment(enrollmentID, amount);
            return StatusCode(response.Status, response);
        }

        [HttpPut("{paymentID}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(int paymentID, string status)
        {
            var response = await _paymentRepository.UpdatePaymentStatus(paymentID, status);
            return StatusCode(response.Status, response);
        }

        [HttpGet("enrollment/{enrollmentID}")]
        public async Task<IActionResult> GetPaymentsByEnrollment(int enrollmentID)
        {
            var response = await _paymentRepository.GetPaymentsByEnrollment(enrollmentID);
            return StatusCode(response.Status, response);
        }

        [HttpGet("{paymentID}")]
        public async Task<IActionResult> GetPaymentByID(int paymentID)
        {
            var response = await _paymentRepository.GetPaymentByID(paymentID);
            return StatusCode(response.Status, response);
        }

        [HttpGet("AllPayment")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _paymentRepository.GetAllPayments();
            return StatusCode(response.Status, response);
        }
    }
}