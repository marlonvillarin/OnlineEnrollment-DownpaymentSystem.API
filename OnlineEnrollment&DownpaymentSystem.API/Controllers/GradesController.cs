using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly IGradesRepository _gradesRepository;

        public GradesController(IGradesRepository gradesRepository)
        {
            _gradesRepository = gradesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade(int studentID, string subjectName, string schoolYear, string semester, decimal? grade, string remarks)
        {
            var response = await _gradesRepository.AddGrade(studentID, subjectName, schoolYear, semester, grade, remarks);
            return StatusCode(response.Status, response);
        }

        [HttpPut("{gradeID}")]
        public async Task<IActionResult> UpdateGrade(int gradeID, decimal? grade, string remarks)
        {
            var response = await _gradesRepository.UpdateGrade(gradeID, grade, remarks);
            return StatusCode(response.Status, response);
        }

        [HttpGet("student/{studentID}")]
        public async Task<IActionResult> GetGradesByStudent(int studentID)
        {
            var response = await _gradesRepository.GetGradesByStudent(studentID);
            return StatusCode(response.Status, response);
        }

        [HttpGet("{gradeID}")]
        public async Task<IActionResult> GetGradeByID(int gradeID)
        {
            var response = await _gradesRepository.GetGradeByID(gradeID);
            return StatusCode(response.Status, response);
        }
    }
}