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
                param.Add(username);
                param.Add(password);

                var result = conn.Query("SP_ENROLLMENT_GETUSERLOGIN", param, commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex) { }

            return service;
        }
    }
}
