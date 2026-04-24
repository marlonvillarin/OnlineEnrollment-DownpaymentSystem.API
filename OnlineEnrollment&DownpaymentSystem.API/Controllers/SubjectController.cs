using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepo;

        public SubjectController(ISubjectRepository subjectRepo)
        {
            _subjectRepo = subjectRepo;
        }

        [HttpPost]
        public async Task<IActionResult> AddSubject(SubjectModel subject)
        {
            var result = await _subjectRepo.AddSubject(subject);
            return StatusCode(result.Status, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _subjectRepo.GetAllSubjects();
            return StatusCode(result.Status, result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetByFilter(string course, int yearLevel, string semester)
        {
            var result = await _subjectRepo.GetSubjectsByFilter(course, yearLevel, semester);
            return StatusCode(result.Status, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(SubjectModel subject)
        {
            var result = await _subjectRepo.UpdateSubject(subject);
            return StatusCode(result.Status, result);
        }

        [HttpDelete("{subjectID}")]
        public async Task<IActionResult> Delete(int subjectID)
        {
            var result = await _subjectRepo.DeleteSubject(subjectID);
            return StatusCode(result.Status, result);
        }
    }
}
