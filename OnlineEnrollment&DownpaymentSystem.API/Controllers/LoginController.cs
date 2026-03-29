using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository _loginRepository;

        public LoginController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [HttpPost("Creating/Student/{studentID}")]
        public async Task<IActionResult> CreateLogin(int studentID, [FromBody] StudentCreateModel login)
        {
            var response = await _loginRepository.CreateLogin(studentID, login.Username, login.Password);
            return StatusCode(response.Status, response);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(string username, string password)
        {
            var response = await _loginRepository.Authenticate(username, password);
            return StatusCode(response.Status, response);
        }
    }
}