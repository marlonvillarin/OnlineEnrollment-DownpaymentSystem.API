using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginRepository _studentLoginRepo;
        private readonly IUserLoginRepository _staffLoginRepo;

        public AuthController(ILoginRepository studentLoginRepo, IUserLoginRepository staffLoginRepo)
        {
            _studentLoginRepo = studentLoginRepo;
            _staffLoginRepo = staffLoginRepo;
        }

        [HttpPost("login")]
        //Student & Staff
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var studentResp = await _studentLoginRepo.Authenticate(login.Username, login.Password);
            if (studentResp.Status == 200) return Ok(studentResp);

            var staffResp = await _staffLoginRepo.Authenticate(login.Username, login.Password);
            if (staffResp.Status == 200) return Ok(staffResp);

            return Unauthorized(new { Message = "Invalid username or password" });
        }

        [HttpPost("create-student")]
        [Authorize(Roles = "Admin,Registrar,Cashier")]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateModel studentLogin)
        {
            var resp = await _studentLoginRepo.CreateLogin(studentLogin.StudentID, studentLogin.Username, studentLogin.Password);
            return StatusCode(resp.Status, resp);
        }

        [HttpPost("create-staff")]
        public async Task<IActionResult> CreateStaff([FromBody] StaffCreateModel staffLogin)
        {
            var resp = await _staffLoginRepo.CreateLogin(staffLogin.Username, staffLogin.Password, staffLogin.Role);
            return StatusCode(resp.Status, resp);
        }
    }

 
}