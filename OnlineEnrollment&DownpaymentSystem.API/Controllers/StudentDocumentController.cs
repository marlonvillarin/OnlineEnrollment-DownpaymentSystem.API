using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentDocumentController : ControllerBase
    {
        private readonly IStudentDocumentRepository _studentDocumentRepository;

        public StudentDocumentController(IStudentDocumentRepository studentDocumentRepository)
        {
            _studentDocumentRepository = studentDocumentRepository;
        }

        
        [HttpPost]
        [Route("InsertStudentDocument/{studentId}")]
        public async Task<IActionResult> InsertStudentDocument(int studentId, DocumentModel studentDocument)
        {
            studentDocument.StudentID = studentId;

            var response = await _studentDocumentRepository.InsertStudentDocument(studentDocument);

            if (response.Status == 200)
                return Ok(response);

            if (response.Status == 400)
                return BadRequest(response.Message);

            return StatusCode(500, response.Message);
        }
       
        [HttpGet("AllDocuments")]
        public async Task<IActionResult> GetAllDocuments()
        {
            var response = await _studentDocumentRepository.GetAllDocuments();
            return StatusCode(response.Status, response);
        }

        
  
    }
}
