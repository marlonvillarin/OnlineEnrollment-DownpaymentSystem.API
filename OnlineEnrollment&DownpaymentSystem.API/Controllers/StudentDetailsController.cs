using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentDetailsController : ControllerBase
    {
        private readonly IStudentDetailsRepository _studentDetailsRepository;

        public StudentDetailsController(IStudentDetailsRepository studentDetailsRepository)
        {
            _studentDetailsRepository = studentDetailsRepository;
        }

        [HttpGet("students-with-status")]
        public async Task<IActionResult> GetAllStudentsWithStatus(
           [FromQuery] string search = null,
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10,
           [FromQuery] string studentStatus = null,
           [FromQuery] string enrollmentStatus = null,
           [FromQuery] string paymentStatus = null,
           [FromQuery] string documentStatus = null,
           [FromQuery] string course = null,
           [FromQuery] string yearLevel = null,
           [FromQuery] string studentType = null,
           [FromQuery] string overallStatus = null)  
        {
            var response = await _studentDetailsRepository.GetAllStudentsWithStatusAsync(
                search, pageNumber, pageSize, studentStatus, enrollmentStatus,
                paymentStatus, documentStatus, course, yearLevel, studentType, overallStatus);
            return StatusCode(response.Status, response);
        }


        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveStudent(int id)
        {
            var response = await _studentDetailsRepository.ApproveStudentAsync(id);
            return StatusCode(response.Status, response);
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> RejectStudent(int id)
        {
            var response = await _studentDetailsRepository.RejectStudentAsync(id);
            return StatusCode(response.Status, response);
        }
    }
}
