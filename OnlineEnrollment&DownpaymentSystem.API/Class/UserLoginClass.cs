using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data;
using System.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class UserLoginClass : IUserLoginRepository
    {
        private readonly SqlConnection conn;

        public UserLoginClass(IConfiguration config)
        {
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
        }

        public async Task<ServiceResponse<UserLoginModel>> CreateLogin(string username, string password, string role)
        {
            var service = new ServiceResponse<UserLoginModel>();
            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                var param = new DynamicParameters();
                param.Add("@Username", username);
                param.Add("@PasswordHash", hashedPassword);
                param.Add("@Role", role);
                param.Add("@StatementType", "INSERT");

                var userID = await conn.QueryFirstOrDefaultAsync<int>(
                    "SP_USERLOGIN", param, commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Message = "Staff login created successfully";
                service.Data = new UserLoginModel
                {
                    UserID = userID,
                    Username = username,
                    PasswordHash = hashedPassword,
                    Role = role
                };
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<ServiceResponse<UserLoginModel>> Authenticate(string username, string password)
        {
            var service = new ServiceResponse<UserLoginModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@Username", username);
                param.Add("@StatementType", "GETBYUSERNAME");

                var user = await conn.QueryFirstOrDefaultAsync<UserLoginModel>(
                    "SP_USERLOGIN", param, commandType: CommandType.StoredProcedure
                );

                if (user == null)
                {
                    service.Status = 400;
                    service.Message = "User not found";
                    return service;
                }

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
                service.Data = user;
                service.Token = token;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        private string GenerateToken(UserLoginModel user)
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
                new Claim("UserID", user.UserID.ToString()),
                new Claim("Role", user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}