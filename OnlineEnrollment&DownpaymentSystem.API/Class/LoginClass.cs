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
        private readonly EmailService _emailService; 

        public LoginClass(IConfiguration config, EmailService emailService) 
        {
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
            _emailService = emailService; 
        }

        public async Task<ServiceResponse<StudentLoginModel>> CreateLogin(int studentID, string username, string password)
        {
            var service = new ServiceResponse<StudentLoginModel>();
            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                var param = new DynamicParameters();
                param.Add("@StudentID", studentID);
                param.Add("@Username", username);
                param.Add("@PasswordHash", hashedPassword);
                param.Add("@StatementType", "INSERT");

                var loginID = await conn.QueryFirstOrDefaultAsync<int>(
                    "SP_STUDENTLOGIN", param, commandType: CommandType.StoredProcedure
                );

              
                var student = await GetStudentInfo(studentID);

                if (!string.IsNullOrEmpty(student.Email))
                {
                    _ = Task.Run(async () =>
                    {
                        await _emailService.SendCredentialsEmail(
                            student.Email,
                            username,
                            password,
                            student.FullName
                        );
                    });
                }

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

        private async Task<(string Email, string FullName)> GetStudentInfo(int studentId)
        {
            var sql = "SELECT Email, FirstName + ' ' + LastName AS FullName FROM TBL_Student WHERE StudentID = @StudentID";
            var result = await conn.QueryFirstOrDefaultAsync<dynamic>(sql, new { StudentID = studentId });

            if (result != null)
            {
                return (result.Email, result.FullName);
            }
            return (string.Empty, string.Empty);
        }

        public async Task<ServiceResponse<List<StudentAccountListModel>>> GetAllStudentAccountsAsync(string searchTerm = null)
        {
            var service = new ServiceResponse<List<StudentAccountListModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@SearchTerm", searchTerm);

                var result = await conn.QueryAsync<StudentAccountListModel>(
                    "SP_GET_ALL_STUDENT_ACCOUNTS",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Data = result.ToList();
              
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
                    return new ServiceResponse<StudentLoginModel> { Status = 400, Message = "User not found" };

                bool verified = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
                if (!verified)
                    return new ServiceResponse<StudentLoginModel> { Status = 401, Message = "Invalid password" };

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

        public async Task<ServiceResponse<StudentAccountModel>> GetStudentByIdAsync(int studentId)
        {
            var service = new ServiceResponse<StudentAccountModel>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", studentId);
                param.Add("@StatementType", "GETBYID");

                var result = await conn.QueryFirstOrDefaultAsync<StudentAccountModel>(
                    "SP_STUDENT",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null)
                {
                    service.Status = 200;
                    service.Data = result;
                    
                }
                else
                {
                    service.Status = 404;
                    service.Message = "Student not found";
                }
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<bool> AccountExistsAsync(int studentId)
        {
            var param = new DynamicParameters();
            param.Add("@StudentID", studentId);
            param.Add("@StatementType", "CHECKEXISTS");

            var result = await conn.QueryFirstOrDefaultAsync<int>(
                "SP_STUDENTLOGIN", param, commandType: CommandType.StoredProcedure
            );

            return result > 0;
        }
        private string GenerateToken(StudentLoginModel user)
        {
            var jwtSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
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
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}