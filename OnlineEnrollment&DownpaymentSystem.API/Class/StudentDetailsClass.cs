using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data;
using System.Data.SqlClient;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class StudentDetailsClass : IStudentDetailsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection conn;
        private readonly EmailService _emailService;
        public StudentDetailsClass(IConfiguration config, EmailService emailService)
        {
            _configuration = config;
            conn = new SqlConnection(_configuration["ConnectionString:Enrollmentdb"]);
            _emailService = emailService;
        }
        public async Task<ServiceResponse<StudentsWithStatusResponse>> GetAllStudentsWithStatusAsync(
            string searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10,
            string studentStatusFilter = null,
            string enrollmentStatusFilter = null,
            string paymentStatusFilter = null,
            string documentStatusFilter = null,
            string courseFilter = null,
            string yearLevelFilter = null,
            string studentTypeFilter = null,
            string overallStatusFilter = null) 
        {
            var service = new ServiceResponse<StudentsWithStatusResponse>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@SearchTerm", searchTerm);
                param.Add("@PageNumber", pageNumber);
                param.Add("@PageSize", pageSize);
                param.Add("@StudentStatusFilter", studentStatusFilter);
                param.Add("@EnrollmentStatusFilter", enrollmentStatusFilter);
                param.Add("@PaymentStatusFilter", paymentStatusFilter);
                param.Add("@DocumentStatusFilter", documentStatusFilter);
                param.Add("@CourseFilter", courseFilter);
                param.Add("@YearLevelFilter", yearLevelFilter);
                param.Add("@StudentTypeFilter", studentTypeFilter);
                param.Add("@OverallStatusFilter", overallStatusFilter);  

                var result = await conn.QueryAsync<StudentDetailsModel>(
                    "SP_GET_ALL_STUDENTS_WITH_STATUS",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                var students = result.ToList();
                var totalCount = students.FirstOrDefault()?.TotalCount ?? 0;

                service.Status = 200;
                service.Data = new StudentsWithStatusResponse
                {
                    Students = students,
                    TotalCount = totalCount,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };
                service.Message = $"Found {students.Count} students";
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<ServiceResponse<StudentModel>> ApproveStudentAsync(int studentId)
        {
            var service = new ServiceResponse<StudentModel>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "APPROVE");
                param.Add("@StudentID", studentId);
                
                var result = await conn.QueryFirstOrDefaultAsync<StudentModel>(
                    "SP_STUDENT_STATUS",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null)
                {
                    var studentInfo = await GetStudentInfo(studentId);
                    if (!string.IsNullOrEmpty(studentInfo.Email))
                    {
                        await _emailService.SendStudentApprovalEmail(
                            studentInfo.Email,
                            studentInfo.FullName,
                            result.StudentNumber ?? ""
                        );
                    }

                    service.Status = 200;
                    service.Data = result;
                 
                    service.Message = "Student approved successfully";
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

        public async Task<ServiceResponse<StudentModel>> RejectStudentAsync(int studentId)
        {
            var service = new ServiceResponse<StudentModel>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "REJECT");
                param.Add("@StudentID", studentId);
               

                var result = await conn.QueryFirstOrDefaultAsync<StudentModel>(
                    "SP_STUDENT_STATUS",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null)
                {

                 
                    var studentInfo = await GetStudentInfo(studentId);
                    if (!string.IsNullOrEmpty(studentInfo.Email))
                    {
                        await _emailService.SendStudentRejectionEmail(
                            studentInfo.Email,
                            studentInfo.FullName,
                            result.StudentNumber ?? "",
                            "Application does not meet the admission requirements."
                        );
                    }

                    service.Status = 200;
                    service.Data = result;
                    service.Message = "Student rejected";
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
    }
}
    