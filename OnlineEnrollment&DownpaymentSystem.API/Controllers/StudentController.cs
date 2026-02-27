using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
   public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentProfile(int StudentID)
        {
            ServiceResponse<StudentProfileModel> response = await _studentRepository.GetStudentProfile(StudentID);  

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