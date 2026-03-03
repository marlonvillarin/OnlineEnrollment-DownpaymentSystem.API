using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data.SqlClient;
using System.Data;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class ValidationClass : IValidationRepository
    {
        private readonly SqlConnection conn;

        public ValidationClass(IConfiguration config)
        {
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
        }

        public async Task<ServiceResponse<ValidationModel>> ValidateStudentForEnrollment(int studentID)
        {
            var service = new ServiceResponse<ValidationModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", studentID);

                var result = await conn.QueryFirstOrDefaultAsync<ValidationModel>(
                    "SP_VALIDATION", param, commandType: CommandType.StoredProcedure
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