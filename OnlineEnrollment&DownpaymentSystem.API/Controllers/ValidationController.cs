using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidationController : ControllerBase
    {
        private readonly IValidationRepository _validationRepository;

        public ValidationController(IValidationRepository validationRepository)
        {
            _validationRepository = validationRepository;
        }

        [HttpGet("{studentID}")]
        public async Task<IActionResult> ValidateStudent(int studentID)
        {
            var response = await _validationRepository.ValidateStudentForEnrollment(studentID);
            return StatusCode(response.Status, response);
        }
    }
}