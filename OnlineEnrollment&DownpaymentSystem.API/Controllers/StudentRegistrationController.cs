using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using BCrypt.Net;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentRegistrationController : ControllerBase
    {
        private readonly IStudentRegisterRepository _adminRepository;

        public StudentRegistrationController(IStudentRegisterRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            // Hash the password BEFORE passing it to the repository
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // Create a new RegisterAccountModel with the hashed password
            var registrationModel = new RegisterAccountModel
            {
                Username = model.Username,
                Password = hashedPassword, // Use the hashed password
                ConfirmPassword = hashedPassword, //This can be the same hash
                StudentID = model.StudentID
            };

            var response = await _adminRepository.RegisterAdmin(registrationModel);

            return StatusCode(response.Status, response);
        }
    }
}