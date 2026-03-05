using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentModel student)
        {
            var response = await _studentRepository.CreateStudent(student);
            return StatusCode(response.Status, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentModel student)
        {
            var response = await _studentRepository.UpdateStudent(student);
            return StatusCode(response.Status, response);
        }

        [HttpGet("{studentID}")]
        
        public async Task<IActionResult> GetStudent(int studentID)
        {
            var response = await _studentRepository.GetStudentByID(studentID);
            return StatusCode(response.Status, response);
        }

        [HttpPost("documents")]
        public async Task<IActionResult> UploadDocument(int studentID, string documentType, string filePath)
        {
            var response = await _studentRepository.UploadDocument(studentID, documentType, filePath);
            return StatusCode(response.Status, response);
        }

        [HttpGet("documents/{studentID}")]
        public async Task<IActionResult> GetDocuments(int studentID)
        {
            var response = await _studentRepository.GetDocumentsByStudent(studentID);
            return StatusCode(response.Status, response);
        }
    }
}