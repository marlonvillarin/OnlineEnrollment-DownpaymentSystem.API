using Microsoft.AspNetCore.Mvc;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification(int studentID, string message)
        {
            var response = await _notificationRepository.CreateNotification(studentID, message);
            return StatusCode(response.Status, response);
        }

        [HttpGet("{studentID}")]
        public async Task<IActionResult> GetNotifications(int studentID)
        {
            var response = await _notificationRepository.GetNotificationsByStudent(studentID);
            return StatusCode(response.Status, response);
        }

        [HttpPut("read/{notificationID}")]
        public async Task<IActionResult> MarkAsRead(int notificationID)
        {
            var response = await _notificationRepository.MarkAsRead(notificationID);
            return StatusCode(response.Status, response);
        }
    }
}