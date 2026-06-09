using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Threading.Tasks;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EligibilityController : ControllerBase
    {
        private readonly IEligibilityRepository _eligibilityRepository;

        public EligibilityController(IEligibilityRepository eligibilityRepository)
        {
            _eligibilityRepository = eligibilityRepository;
        }

        [HttpGet("check-eligibility")]
        public async Task<IActionResult> CheckEligibility(
      [FromQuery] string studentNumber,
      [FromQuery] string schoolYear,
      [FromQuery] string semester)
        {
            var response = await _eligibilityRepository.CheckEligibilityAsync(studentNumber, schoolYear, semester);
            return StatusCode(response.Status, response);
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetStudentsForRegistrar([FromQuery] string schoolYear, [FromQuery] string semester)
        {
            var response = await _eligibilityRepository.GetStudentsForRegistrarAsync(schoolYear, semester);
            return StatusCode(response.Status, response);
        }

        [HttpPost("save-eligibility")]
        public async Task<IActionResult> SaveEligibility([FromBody] RegistrarEligibilityModel eligibility)
        {
            var response = await _eligibilityRepository.SaveRegistrarEligibilityAsync(eligibility);
            return StatusCode(response.Status, response);
        }
    }
}