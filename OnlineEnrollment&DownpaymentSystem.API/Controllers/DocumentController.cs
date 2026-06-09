using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IStudentDocumentRepository _studentDocumentRepository;

        public DocumentController(IStudentDocumentRepository studentDocumentRepository)
        {
            _studentDocumentRepository = studentDocumentRepository;
        }

        [HttpGet("documents/all")]
        public async Task<IActionResult> GetAllDocuments([FromQuery] string search = null)
        {
            var response = await _studentDocumentRepository.GetAllDocumentsWithStudentsAsync(search);
            return StatusCode(response.Status, response);
        }

        [HttpGet("documents/pending")]
        public async Task<IActionResult> GetPendingDocuments([FromQuery] string search = null)
        {
            var response = await _studentDocumentRepository.GetPendingDocumentsWithStudentsAsync(search);
            return StatusCode(response.Status, response);
        }

        [HttpGet("documents/approved")]
        public async Task<IActionResult> GetApprovedDocuments([FromQuery] string search = null)
        {
            var response = await _studentDocumentRepository.GetApprovedDocumentsWithStudentsAsync(search);
            return StatusCode(response.Status, response);
        }

        [HttpPost("documents/approve/{id}")]
        public async Task<IActionResult> ApproveDocument(int id)
        {
            var response = await _studentDocumentRepository.ApproveDocumentAsync(id);
            return StatusCode(response.Status, response);
        }

        [HttpPost("documents/reject/{id}")]
        public async Task<IActionResult> RejectDocument(int id)
        {
            var response = await _studentDocumentRepository.RejectDocumentAsync(id);
            return StatusCode(response.Status, response);
        }

        [HttpGet("documents/{id}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            var response = await _studentDocumentRepository.GetDocumentById(id);
            return StatusCode(response.Status, response);
        }

        [HttpGet("documents/student/{studentId}")]
        public async Task<IActionResult> GetDocumentsByStudent(int studentId)
        {
            var response = await _studentDocumentRepository.GetDocumentsByStudentAsync(studentId);
            return StatusCode(response.Status, response);
        }

        [HttpGet("documents/view/{documentId}")]
        public async Task<IActionResult> ViewDocument(int documentId)
        {
            var response = await _studentDocumentRepository.GetDocumentById(documentId);

            if (response.Data == null || string.IsNullOrEmpty(response.Data.FilePath))
            {
                return NotFound(new { message = "Document not found" });
            }

            if (!System.IO.File.Exists(response.Data.FilePath))
            {
                return NotFound(new { message = "File not found on server" });
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(response.Data.FilePath);
            var fileName = Path.GetFileName(response.Data.FilePath);
            var contentType = GetContentType(fileName);

            return File(fileBytes, contentType, fileName);
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream"
            };
        }

        [HttpGet("documents/view-image/{documentId}")]
        public async Task<IActionResult> ViewImage(int documentId)
        {
            var response = await _studentDocumentRepository.GetDocumentById(documentId);

            if (response.Data == null || string.IsNullOrEmpty(response.Data.FilePath))
            {
                return NotFound(new { message = "Document not found" });
            }

            if (!System.IO.File.Exists(response.Data.FilePath))
            {
                return NotFound(new { message = "File not found on server" });
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(response.Data.FilePath);
            var fileName = Path.GetFileName(response.Data.FilePath);
            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            // For images, display in browser (not download)
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                var contentType = extension == ".png" ? "image/png" : "image/jpeg";
                return File(fileBytes, contentType);
            }
            else
            {
                // For PDF or other files, download
                var contentType = GetContentType(fileName);
                return File(fileBytes, contentType, fileName);
            }
        }
    }
}
