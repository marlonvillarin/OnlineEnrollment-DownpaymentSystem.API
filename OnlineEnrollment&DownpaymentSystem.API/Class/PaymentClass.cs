using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data.SqlClient;
using System.Data;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class PaymentClass : IPaymentRepository
    {
        private readonly SqlConnection conn;

        public PaymentClass(IConfiguration config)
        {
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
        }

        public async Task<ServiceResponse<PaymentModel>> CreatePayment(int enrollmentID, decimal amount)
        {
            var service = new ServiceResponse<PaymentModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@EnrollmentID", enrollmentID);
                param.Add("@Amount", amount);
                param.Add("@StatementType", "INSERT");

                var result = await conn.QueryFirstOrDefaultAsync<PaymentModel>(
                    "SP_PAYMENTS", param, commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Message = "Payment created successfully";
                service.Data = result;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<ServiceResponse<PaymentModel>> UpdatePaymentStatus(int paymentID, string status)
        {
            var service = new ServiceResponse<PaymentModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@PaymentID", paymentID);
                param.Add("@PaymentStatus", status);
                param.Add("@StatementType", "UPDATESTATUS");

                var result = await conn.QueryFirstOrDefaultAsync<PaymentModel>(
                    "SP_PAYMENTS", param, commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Message = "Payment status updated successfully";
                service.Data = result;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<ServiceResponse<List<PaymentModel>>> GetPaymentsByEnrollment(int enrollmentID)
        {
            var service = new ServiceResponse<List<PaymentModel>>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@EnrollmentID", enrollmentID);
                param.Add("@StatementType", "GETBYENROLLMENT");

                var result = (await conn.QueryAsync<PaymentModel>(
                    "SP_PAYMENTS", param, commandType: CommandType.StoredProcedure
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

        public async Task<ServiceResponse<PaymentModel>> GetPaymentByID(int paymentID)
        {
            var service = new ServiceResponse<PaymentModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@PaymentID", paymentID);
                param.Add("@StatementType", "GETBYID");

                var result = await conn.QueryFirstOrDefaultAsync<PaymentModel>(
                    "SP_PAYMENTS", param, commandType: CommandType.StoredProcedure
                );

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
    }
}