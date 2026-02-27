using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BCrypt.Net;


namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class LoginClass : ILoginRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection conn;

        public LoginClass(IConfiguration config) 
        {
            _configuration = config;
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
          }

        public async Task<ServiceResponse<object>> GetLogin(string username, string PasswordHash)
        {
            var service = new ServiceResponse<object>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@Username", username);
                param.Add("@PasswordHash", PasswordHash);
                param.Add("@StatementType", "GETLOGIN");

                var result = conn.Query("SP_ENROLLMENT_GETUSERLOGIN", param, commandType: CommandType.StoredProcedure).ToList();
                if (result.Count > 0)
                {
                    service.Status = 200;
                    service.Data = result;
                }
                else
                {
                    service.Status = 400;
                    service.Message = "No Record Found";
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
