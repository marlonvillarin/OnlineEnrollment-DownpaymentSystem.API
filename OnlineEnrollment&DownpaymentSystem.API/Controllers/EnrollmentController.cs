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
        public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentStudent enrollment)
        {
            var response = await _enrollmentRepository.CreateEnrollment(enrollment);
            return StatusCode(response.Status, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEnrollment([FromBody] EnrollmentStudent enrollment)
        {
            var response = await _enrollmentRepository.UpdateEnrollment(enrollment);
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



        //new
        [HttpGet("all")]
        public async Task<IActionResult> GetAllEnrollmentsWithStudentInfo()
        {
            var response = await _enrollmentRepository.GetAllEnrollmentsWithStudentInfoAsync();
            return StatusCode(response.Status, response);
        }

        [HttpGet("admin-pending")]
        public async Task<IActionResult> GetAdminPendingEnrollments()
        {
            var response = await _enrollmentRepository.GetAdminPendingEnrollmentsAsync();
            return StatusCode(response.Status, response);
        }

        [HttpGet("approved")]
        public async Task<IActionResult> GetApprovedEnrollments()
        {
            var response = await _enrollmentRepository.GetApprovedEnrollmentsAsync();
            return StatusCode(response.Status, response);
        }

        [HttpPost("admin-approve/{id}")]
        public async Task<IActionResult> AdminApproveEnrollment(int id)
        {
            var response = await _enrollmentRepository.AdminApproveEnrollmentAsync(id);
            return StatusCode(response.Status, response);
        }

        [HttpPost("admin-reject/{id}")]
        public async Task<IActionResult> AdminRejectEnrollment(int id)
        {
            var response = await _enrollmentRepository.AdminRejectEnrollmentAsync(id);
            return StatusCode(response.Status, response);
        }
    }
}