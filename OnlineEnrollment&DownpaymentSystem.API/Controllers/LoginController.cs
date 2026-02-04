using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        ILoginRepository loginRepository;
        public LoginController(ILoginRepository login) 
        {
        loginRepository = login;
        }


        [HttpGet]
        public async Task<IActionResult> LoginStudent(LoginModel login)
        {
            LoginModel model = new LoginModel();
            var response = loginRepository.GetLogin(login.Username, login.Password);
            return BadRequest();
        }
    }
}
