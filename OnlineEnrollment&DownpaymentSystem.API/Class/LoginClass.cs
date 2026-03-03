using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data.SqlClient;
using System.Data;
using BCrypt.Net;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class LoginClass : ILoginRepository
    {
        private readonly SqlConnection conn;

        public LoginClass(IConfiguration config)
        {
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
        }

        public async Task<ServiceResponse<StudentLoginModel>> CreateLogin(int studentID, string username, string password)
        {
            var service = new ServiceResponse<StudentLoginModel>();
            try
            {
                // Hash the password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                var param = new DynamicParameters();
                param.Add("@StudentID", studentID);
                param.Add("@Username", username);
                param.Add("@PasswordHash", hashedPassword);
                param.Add("@StatementType", "INSERT");

                var loginID = await conn.QueryFirstOrDefaultAsync<int>(
                    "SP_STUDENTLOGIN", param, commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Message = "Login created successfully";
                service.Data = new StudentLoginModel
                {
                    LoginID = loginID,
                    StudentID = studentID,
                    Username = username,
                    PasswordHash = hashedPassword
                };
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<ServiceResponse<StudentLoginModel>> Authenticate(string username, string password)
        {
            var service = new ServiceResponse<StudentLoginModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@Username", username);
                param.Add("@StatementType", "GETBYUSERNAME");

                var user = await conn.QueryFirstOrDefaultAsync<StudentLoginModel>(
                    "SP_STUDENTLOGIN", param, commandType: CommandType.StoredProcedure
                );

                if (user == null)
                {
                    service.Status = 400;
                    service.Message = "User not found";
                    return service;
                }

                // Verify password
                bool verified = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

                if (!verified)
                {
                    service.Status = 401;
                    service.Message = "Invalid password";
                    return service;
                }

                service.Status = 200;
                service.Message = "Authentication successful";
                service.Data = user;
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