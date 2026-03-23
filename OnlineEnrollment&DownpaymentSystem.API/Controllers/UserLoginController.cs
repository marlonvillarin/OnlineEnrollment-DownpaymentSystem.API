using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserLoginController : ControllerBase
    {
        private readonly IUserLoginRepository _userLoginRepo;

        public UserLoginController(IUserLoginRepository userLoginRepo)
        {
            _userLoginRepo = userLoginRepo;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateLogin(string username, string password, string role)
        {
            var response = await _userLoginRepo.CreateLogin(username, password, role);
            return StatusCode(response.Status, response);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(string username, string password)
        {
            var response = await _userLoginRepo.Authenticate(username, password);
            return StatusCode(response.Status, response);
        }
    }
}