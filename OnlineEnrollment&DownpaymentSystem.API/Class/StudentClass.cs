using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.Controllers;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using System.Data;
using System.Data.SqlClient;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class StudentClass : IStudentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection conn;

        public StudentClass(IConfiguration config)
        {
            _configuration = config;
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
        }
        public async Task<ServiceResponse<StudentProfileModel>> GetStudentProfile(int StudentID)
        {
            ServiceResponse<StudentProfileModel> service = new ServiceResponse<StudentProfileModel>();
            try
            {
                // NEW: Validate studentId BEFORE querying the database**
                if (StudentID <= 0) 
                {
                    service.Status = 400; // Bad Request
                    service.Message = "Invalid StudentID";
                    return service; // Return early
                }

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@StudentID", StudentID, DbType.Int32);


                StudentProfileModel student = await conn.QueryFirstOrDefaultAsync<StudentProfileModel>("SP_GetStudentProfile", parameters, commandType: CommandType.StoredProcedure);

                if (student != null)
                {
                    service.Status = 200;
                    service.Message = "Student profile retrieved successfully";
                    service.Data = student;
                }
                else
                {
                    service.Status = 404;
                    service.Message = "Student not found";
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000 && ex.Message.Contains("Student not found"))
                {
                   
                    service.Status = 404;
                    service.Message = "Student not found";
                }
                else
                {
                    
                    service.Status = 500;
                    service.Message = ex.Message;
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
