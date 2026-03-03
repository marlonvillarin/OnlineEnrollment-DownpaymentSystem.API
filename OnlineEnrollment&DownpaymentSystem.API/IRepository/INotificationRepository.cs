using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface INotificationRepository
    {
        Task<ServiceResponse<NotificationModel>> CreateNotification(int studentID, string message);
        Task<ServiceResponse<List<NotificationModel>>> GetNotificationsByStudent(int studentID);
        Task<ServiceResponse<NotificationModel>> MarkAsRead(int notificationID);
    }
}