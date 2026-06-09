using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
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
       
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var studentResp = await _studentLoginRepo.Authenticate(login.Username, login.Password);
            if (studentResp.Status == 200) return Ok(studentResp);

            var staffResp = await _staffLoginRepo.Authenticate(login.Username, login.Password);
            if (staffResp.Status == 200) return Ok(staffResp);

            return Unauthorized(new { Message = "Invalid username or password" });
        }

        [HttpPost("create-student")]

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




        [HttpGet("student/{studentId}")]
public async Task<IActionResult>GetStudentByIdAsync(int studentId)
        {
            var response = await _studentLoginRepo.GetStudentByIdAsync(studentId);
            return StatusCode(response.Status, response);
        }

        [HttpGet("account-exists/{studentId}")]
        public async Task<IActionResult> AccountExists(int studentId)
        {
            var exists = await _studentLoginRepo.AccountExistsAsync(studentId);
            return Ok(new ServiceResponse<bool> { Status = 200, Data = exists });
        }

        [HttpGet("all-accounts")]
        public async Task<IActionResult> GetAllStudentAccounts([FromQuery] string search = null)
        {
            var response = await _studentLoginRepo.GetAllStudentAccountsAsync(search);
            return StatusCode(response.Status, response);
        }

    }



 
}