using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentController(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnrollment([FromBody] EnrollmentModel enrollment)
        {
            var response = await _enrollmentRepository.CreateEnrollment(enrollment);
            return StatusCode(response.Status, response);
        }

        [HttpPut("{enrollmentID}/status")]
        public async Task<IActionResult> UpdateStatus(int enrollmentID, [FromBody] string status)
        {
            var response = await _enrollmentRepository.UpdateEnrollmentStatus(enrollmentID, status);
            return StatusCode(response.Status, response);
        }

        [HttpGet("student/{studentID}")]
        public async Task<IActionResult> GetByStudent(int studentID)
        {
            var response = await _enrollmentRepository.GetEnrollmentsByStudent(studentID);
            return StatusCode(response.Status, response);
        }
    }
}