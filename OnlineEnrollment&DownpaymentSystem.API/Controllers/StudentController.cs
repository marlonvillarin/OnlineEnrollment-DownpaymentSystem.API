using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

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
        public async Task<IActionResult> UploadDocument(
      [FromForm] int studentID,
      [FromForm] string documentType,
      IFormFile file)  
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { status = 400, message = "No file uploaded" });
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Documents");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{studentID}_{documentType.Replace(" ", "_")}_{DateTime.Now.Ticks}.jpg";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var response = await _studentRepository.UploadDocument(studentID, documentType, filePath);
                return StatusCode(response.Status, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 500, message = ex.Message });
            }
        }

        [HttpGet("documents/{studentID}")]
        public async Task<IActionResult> GetDocuments(int studentID)
        {
            var response = await _studentRepository.GetDocumentsByStudent(studentID);
            return StatusCode(response.Status, response);
        }

        [HttpGet("AllStudents")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _studentRepository.GetAllStudents();
            return StatusCode(response.Status, response);
        }
    }
}