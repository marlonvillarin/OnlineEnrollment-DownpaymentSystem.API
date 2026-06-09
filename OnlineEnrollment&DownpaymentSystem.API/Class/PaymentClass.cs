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
        
        private readonly EmailService _emailService;

        public PaymentClass(IConfiguration config, EmailService emailService)
        {
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
            _emailService = emailService;
        }
        public async Task<ServiceResponse<PaymentModel>> CreatePayment(
            int enrollmentID,
            decimal amount,
            string? referenceNumber = null,
            string? paymentMethod = null,
            string? remarks = null)
        {
            var service = new ServiceResponse<PaymentModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@EnrollmentID", enrollmentID);
                param.Add("@Amount", amount);
                param.Add("@ReferenceNumber", referenceNumber);
                param.Add("@PaymentMethod", paymentMethod);
                param.Add("@Remarks", remarks);
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

        public async Task<ServiceResponse<List<PaymentModel>>> GetPendingPaymentsAsync(string searchTerm = null)
        {
            var service = new ServiceResponse<List<PaymentModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StatementType", "GETPENDING");
                param.Add("@SearchTerm", searchTerm);

                var result = await conn.QueryAsync<PaymentModel>(
                    "SP_PAYMENTS",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Data = result.ToList();
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

     
        public async Task<ServiceResponse<List<PaymentModel>>> GetApprovedPaymentsAsync(string searchTerm = null)
        {
            var service = new ServiceResponse<List<PaymentModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StatementType", "GETAPPROVED");
                param.Add("@SearchTerm", searchTerm);

                var result = await conn.QueryAsync<PaymentModel>(
                    "SP_PAYMENTS",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Data = result.ToList();
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

     
        public async Task<ServiceResponse<List<PaymentModel>>> GetAllPaymentsAsync(string searchTerm = null)
        {
            var service = new ServiceResponse<List<PaymentModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StatementType", "GETALL");
                param.Add("@SearchTerm", searchTerm);

                var result = await conn.QueryAsync<PaymentModel>(
                    "SP_PAYMENTS",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Data = result.ToList();
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<ServiceResponse<PaymentModel>> ApprovePaymentAsync(int paymentId)
        {
            var service = new ServiceResponse<PaymentModel>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StatementType", "APPROVE");
                param.Add("@PaymentID", paymentId);

                var result = await conn.QueryFirstOrDefaultAsync<PaymentModel>(
                    "SP_PAYMENTS",
                    param,
                    commandType: CommandType.StoredProcedure
                );
                if (result != null)
                {
                  
                    var studentInfo = await GetStudentInfo(result.StudentID);
                    if (!string.IsNullOrEmpty(studentInfo.Email))
                    {
                        await _emailService.SendPaymentApprovalEmail(
                            studentInfo.Email,
                            studentInfo.FullName,
                            result.Amount,
                            result.ReferenceNumber ?? ""
                        );
                    }

                    service.Status = 200;
                    service.Data = result;
                    service.Message = "Payment approved successfully";
                }
                else
                {
                    service.Status = 404;
                    service.Message = "Payment not found";
                }
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<ServiceResponse<PaymentModel>> RejectPaymentAsync(int paymentId)
        {
            var service = new ServiceResponse<PaymentModel>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StatementType", "REJECT");
                param.Add("@PaymentID", paymentId);

                var result = await conn.QueryFirstOrDefaultAsync<PaymentModel>(
                    "SP_PAYMENTS",
                    param,
                    commandType: CommandType.StoredProcedure
                );
                if (result != null)
                {
                   
                    var studentInfo = await GetStudentInfo(result.StudentID);
                    if (!string.IsNullOrEmpty(studentInfo.Email))
                    {
                        await _emailService.SendPaymentRejectionEmail(
                            studentInfo.Email,
                            studentInfo.FullName,
                            result.Amount,
                            result.ReferenceNumber ?? "",
                            "Payment verification failed. Please check your payment details and try again."
                        );
                    }

                    service.Status = 200;
                    service.Data = result;
                    service.Message = "Payment rejected";
                }
                else
                {
                    service.Status = 404;
                    service.Message = "Payment not found";
                }
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

    
        private async Task<(string Email, string FullName)> GetStudentInfo(int studentId)
        {
            var sql = "SELECT Email, FirstName + ' ' + LastName AS FullName FROM TBL_Student WHERE StudentID = @StudentID";
            var result = await conn.QueryFirstOrDefaultAsync<dynamic>(sql, new { StudentID = studentId });

            if (result != null)
            {
                return (result.Email, result.FullName);
            }
            return (string.Empty, string.Empty);
        }
    }
}