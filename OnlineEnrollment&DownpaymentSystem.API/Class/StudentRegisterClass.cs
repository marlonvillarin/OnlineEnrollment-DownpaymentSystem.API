using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using System.Data;
using System.Data.SqlClient;
using BCrypt.Net;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class StudentRegisterClass : IRepository.IStudentRegisterRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection conn; 

        public StudentRegisterClass(IConfiguration configuration)
        {
            _configuration = configuration;
            conn = new SqlConnection(_configuration["ConnectionString:Enrollmentdb"]); 
        }

        public async Task<ServiceResponse<object>> RegisterAdmin(RegisterAccountModel model)
        {
            ServiceResponse<object> service = new ServiceResponse<object>();
            try
            {
                
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                DynamicParameters param = new DynamicParameters();
                param.Add("@Username", model.Username);
                param.Add("@PasswordHash", hashedPassword);
                param.Add("@StudentID", model.StudentID);  // Assuming you have StudentID
                param.Add("@UserID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var result = conn.Query("SP_ENROLLMENT_REGISTERUSER", param, commandType: CommandType.StoredProcedure).ToList();

                int newUserId = param.Get<int>("@UserID");

                if (newUserId > 0)
                {
                    service.Status = 201; // Created
                    service.Message = "Registration successful";
                    service.Data = new { UserId = newUserId, Username = model.Username }; // Optional: Return user data
                }
                else
                {
                    service.Status = 400; // Bad Request
                    service.Message = "Registration failed";
                }

            }
            catch (SqlException ex) when (ex.Number == 2601) // Check for unique constraint violation
            {
                service.Status = 409; // Conflict
                service.Message = "Username already exists";
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