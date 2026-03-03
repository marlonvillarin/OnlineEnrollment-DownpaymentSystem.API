using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data.SqlClient;
using System.Data;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class EnrollmentClass : IEnrollmentRepository
    {
        private readonly SqlConnection conn;

        public EnrollmentClass(IConfiguration config)
        {
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
        }

        public async Task<ServiceResponse<EnrollmentModel>> CreateEnrollment(EnrollmentModel enrollment)
        {
            var service = new ServiceResponse<EnrollmentModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", enrollment.StudentID);
                param.Add("@Course", enrollment.Course);
                param.Add("@SchoolYear", enrollment.SchoolYear);
                param.Add("@Semester", enrollment.Semester);
                param.Add("@StatementType", "INSERT");

                var enrollmentID = await conn.QueryFirstOrDefaultAsync<int>("SP_ENROLLMENT", param, commandType: CommandType.StoredProcedure);

                enrollment.EnrollmentID = enrollmentID;
                enrollment.EnrollmentStatus = "Pending";

                service.Status = 200;
                service.Message = "Enrollment created successfully";
                service.Data = enrollment;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }
            return service;
        }

        public async Task<ServiceResponse<EnrollmentModel>> UpdateEnrollmentStatus(int enrollmentID, string status)
        {
            var service = new ServiceResponse<EnrollmentModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@EnrollmentID", enrollmentID);
                param.Add("@EnrollmentStatus", status);
                param.Add("@StatementType", "UPDATESTATUS");

                await conn.ExecuteAsync("SP_ENROLLMENT", param, commandType: CommandType.StoredProcedure);

                service.Status = 200;
                service.Message = $"Enrollment status updated to {status}";
                service.Data = new EnrollmentModel { EnrollmentID = enrollmentID, EnrollmentStatus = status };
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }
            return service;
        }

        public async Task<ServiceResponse<List<EnrollmentModel>>> GetEnrollmentsByStudent(int studentID)
        {
            var service = new ServiceResponse<List<EnrollmentModel>>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", studentID);
                param.Add("@StatementType", "GETBYSTUDENT");

                var result = (await conn.QueryAsync<EnrollmentModel>("SP_ENROLLMENT", param, commandType: CommandType.StoredProcedure)).ToList();

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