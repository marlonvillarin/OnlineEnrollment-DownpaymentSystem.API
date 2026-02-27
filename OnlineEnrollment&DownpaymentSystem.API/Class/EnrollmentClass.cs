using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using System.Data;
using System.Data.SqlClient;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class EnrollmentClass : IEnrollmentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection conn;

        public EnrollmentClass(IConfiguration configuration)
        {
            _configuration = configuration;
            conn = new SqlConnection(_configuration["ConnectionString:Enrollmentdb"]);
        }
        public async Task<ServiceResponse<List<EnrollmentModel>>> GetEnrollment(int StudentID)
        {
            ServiceResponse<List<EnrollmentModel>> service = new ServiceResponse<List<EnrollmentModel>>();
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@StudentID", StudentID, DbType.Int32);

                var result = conn.Query("SP_GETENROLLMENT", parameters, commandType: CommandType.StoredProcedure).ToList();
                List<EnrollmentModel> enrollments = (List<EnrollmentModel>)await conn.QueryAsync<EnrollmentModel>("SP_GetEnrollment", parameters, commandType: CommandType.StoredProcedure);

                if (enrollments != null && enrollments.Count > 0)
                {
                    service.Status = 200;
                    service.Message = "Enrollments retrieved successfully";
                    service.Data = enrollments;
                }
                else
                {
                    service.Status = 404;
                    service.Message = "No enrollments found for this student";
                    service.Data = null;
                }
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