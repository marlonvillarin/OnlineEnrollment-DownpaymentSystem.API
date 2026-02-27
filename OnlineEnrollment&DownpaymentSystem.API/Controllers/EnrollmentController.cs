using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
        public class EnrollmentController : Controller
        {
             IEnrollmentRepository _enrollmentRepository;

            public EnrollmentController(IEnrollmentRepository enrollmentRepository)
            {
                _enrollmentRepository = enrollmentRepository;
            }

            [HttpGet("{studentId}")]
            public async Task<IActionResult> GetEnrollmentDetails(int studentId)
            {
                ServiceResponse<List<EnrollmentModel>> response = await _enrollmentRepository.GetEnrollment(studentId);

                if (response.Status == 200)
                {
                    return Ok(response.Data);
                }
                else if (response.Status == 404)
                {
                    return NotFound(response.Message);
                }
                else
                {
                    return StatusCode(response.Status, response.Message);
                }
            }
        }
    
}