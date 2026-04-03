using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentController(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        //  CREATE ENROLLMENT
        [HttpPost]
        public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentModel enrollment)
        {
            var response = await _enrollmentRepository.CreateEnrollment(enrollment);
            return StatusCode(response.Status, response);
        }

        //  GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _enrollmentRepository.GetAllEnrollments();
            return StatusCode(response.Status, response);
        }

        // GET PENDING (ADMIN PAGE)
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var response = await _enrollmentRepository.GetPendingEnrollments();
            return StatusCode(response.Status, response);
        }

        //  GET BY STUDENT
        [HttpGet("student/{studentID}")]
        public async Task<IActionResult> GetByStudent(int studentID)
        {
            var response = await _enrollmentRepository.GetEnrollmentsByStudent(studentID);
            return StatusCode(response.Status, response);
        }

        //  APPROVE
        [HttpPost("{enrollmentID}/approve")]
        public async Task<IActionResult> Approve(int enrollmentID)
        {
            var response = await _enrollmentRepository.ApproveEnrollment(enrollmentID);
            return StatusCode(response.Status, response);
        }

        // REJECT
        [HttpPost("{enrollmentID}/reject")]
        public async Task<IActionResult> Reject(int enrollmentID)
        {
            var response = await _enrollmentRepository.RejectEnrollment(enrollmentID);
            return StatusCode(response.Status, response);
        }

        //  COMPLETE 
        [HttpPost("{enrollmentID}/complete")]
        public async Task<IActionResult> Complete(int enrollmentID)
        {
            var response = await _enrollmentRepository.CompleteEnrollment(enrollmentID);
            return StatusCode(response.Status, response);
        }
    }
}