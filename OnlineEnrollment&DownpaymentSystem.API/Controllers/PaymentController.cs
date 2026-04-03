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

        // CREATE PAYMENT (FROM FLUTTER)
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            var response = await _paymentRepository.CreatePayment(request.EnrollmentID, request.Amount);
            return StatusCode(response.Status, response);
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _paymentRepository.GetAllPayments();
            return StatusCode(response.Status, response);
        }

        // GET PENDING (CASHIER PAGE)
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var response = await _paymentRepository.GetPendingPayments();
            return StatusCode(response.Status, response);
        }

        //  GET BY ENROLLMENT
        [HttpGet("enrollment/{enrollmentID}")]
        public async Task<IActionResult> GetPaymentsByEnrollment(int enrollmentID)
        {
            var response = await _paymentRepository.GetPaymentsByEnrollment(enrollmentID);
            return StatusCode(response.Status, response);
        }

        //  GET BY ID
        [HttpGet("{paymentID}")]
        public async Task<IActionResult> GetPaymentByID(int paymentID)
        {
            var response = await _paymentRepository.GetPaymentByID(paymentID);
            return StatusCode(response.Status, response);
        }

        // APPROVE 
        [HttpPost("{paymentID}/approve")]
        public async Task<IActionResult> Approve(int paymentID)
        {
            var response = await _paymentRepository.ApprovePayment(paymentID);
            return StatusCode(response.Status, response);
        }

        // REJECT
        [HttpPost("{paymentID}/reject")]
        public async Task<IActionResult> Reject(int paymentID)
        {
            var response = await _paymentRepository.RejectPayment(paymentID);
            return StatusCode(response.Status, response);
        }
    }
}