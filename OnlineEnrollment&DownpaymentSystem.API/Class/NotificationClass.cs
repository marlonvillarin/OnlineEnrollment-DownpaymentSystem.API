using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data.SqlClient;
using System.Data;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class NotificationClass : INotificationRepository
    {
        private readonly SqlConnection conn;

        public NotificationClass(IConfiguration config)
        {
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
        }

        public async Task<ServiceResponse<NotificationModel>> CreateNotification(int studentID, string message)
        {
            var service = new ServiceResponse<NotificationModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", studentID);
                param.Add("@Message", message);
                param.Add("@StatementType", "INSERT");

                var result = await conn.QueryFirstOrDefaultAsync<NotificationModel>(
                    "SP_NOTIFICATIONS", param, commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Message = "Notification created successfully";
                service.Data = result;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<ServiceResponse<List<NotificationModel>>> GetNotificationsByStudent(int studentID)
        {
            var service = new ServiceResponse<List<NotificationModel>>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", studentID);
                param.Add("@StatementType", "GETBYSTUDENT");

                var result = (await conn.QueryAsync<NotificationModel>(
                    "SP_NOTIFICATIONS", param, commandType: CommandType.StoredProcedure
                )).ToList();

                service.Status = 200;
                service.Data = result;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<ServiceResponse<NotificationModel>> MarkAsRead(int notificationID)
        {
            var service = new ServiceResponse<NotificationModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@NotificationID", notificationID);
                param.Add("@StatementType", "MARKREAD");

                var result = await conn.QueryFirstOrDefaultAsync<NotificationModel>(
                    "SP_NOTIFICATIONS", param, commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Message = "Notification marked as read";
                service.Data = result;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }
    }
}