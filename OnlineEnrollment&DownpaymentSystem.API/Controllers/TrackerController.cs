using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackerController : ControllerBase
    {
        private readonly ITrackerRepository _trackerRepository;

        public TrackerController(ITrackerRepository trackerRepository)
        {
            _trackerRepository = trackerRepository;
        }

        [HttpGet("status/{searchValue}")]
        public async Task<IActionResult> Track(string searchValue)
        {
            var response = await _trackerRepository.TrackApplication(searchValue);
            return StatusCode(response.Status, response);
        }
    }
}
