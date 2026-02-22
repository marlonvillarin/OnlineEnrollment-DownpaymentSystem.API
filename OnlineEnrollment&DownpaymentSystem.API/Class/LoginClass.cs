using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class LoginClass : ILoginRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection conn;
        public LoginClass(IConfiguration config) 
        {
            _configuration = config;
            conn = new SqlConnection(config["ConnectionString:onlineEnrollmentdb"]);
          }

        public async Task<ServiceResponse<object>> GetLogin(string username, string password)
        {
            var service = new ServiceResponse<object>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@username", username);
                param.Add("@password", password);
                param.Add("@statementType", "GETLOGIN");

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
