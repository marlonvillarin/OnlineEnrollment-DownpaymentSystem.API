using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data.SqlClient;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

                string token = GenerateToken(user);

                service.Status = 200;
                service.Message = "Authentication successful";
                service.Data = new StudentLoginModel
                {
                    LoginID = user.LoginID,
                    StudentID = user.StudentID,
                    Username = user.Username,
                    PasswordHash = user.PasswordHash
                };

                // You can add token property in ServiceResponse
                service.Token = token;
            }

            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        private string GenerateToken(StudentLoginModel user)
        {
            var jwtSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim("StudentID", user.StudentID.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(jwtSettings["ExpiryMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}