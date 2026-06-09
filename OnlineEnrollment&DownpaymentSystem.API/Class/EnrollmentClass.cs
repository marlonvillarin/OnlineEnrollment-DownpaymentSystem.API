using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data.SqlClient;
using System.Data;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class EnrollmentClass : IEnrollmentRepository
    {
        private readonly SqlConnection conn;
        private readonly EmailService _emailService;

        public EnrollmentClass(IConfiguration config, EmailService emailService)
        {
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
            _emailService = emailService;
        }

        public async Task<ServiceResponse<EnrollmentStudent>> CreateEnrollment(EnrollmentStudent enrollment)
        {
            var service = new ServiceResponse<EnrollmentStudent>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", enrollment.StudentID);
                param.Add("@Course", enrollment.Course);
                param.Add("@YearLevel", enrollment.YearLevel);
                param.Add("@SchoolYear", enrollment.SchoolYear);
                param.Add("@Semester", enrollment.Semester);
                param.Add("@EnrollmentStatus", "Pending");
                param.Add("@StatementType", "INSERT");

               
                var result = await conn.QueryFirstOrDefaultAsync<EnrollmentStudent>(
                    "SP_ENROLLMENT",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null)
                {
                    service.Status = 200;
                    service.Message = "Enrollment created successfully";
                    service.Data = result; 
                }
                else
                {
                    service.Status = 400;
                    service.Message = "Failed to create enrollment";
                }
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }
        public async Task<ServiceResponse<EnrollmentStudent>> UpdateEnrollment(EnrollmentStudent enrollment)
        {
            var service = new ServiceResponse<EnrollmentStudent>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@EnrollmentID", enrollment.EnrollmentID);
                param.Add("@StudentID", enrollment.StudentID);
                param.Add("@Course", enrollment.Course);
                param.Add("@YearLevel", enrollment.YearLevel);
                param.Add("@SchoolYear", enrollment.SchoolYear);
                param.Add("@Semester", enrollment.Semester);
                param.Add("@EnrollmentStatus", enrollment.EnrollmentStatus);
                param.Add("@StatementType", "UPDATE");

                await conn.ExecuteAsync(
                    "SP_ENROLLMENT",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Message = "Enrollment updated successfully";
                service.Data = enrollment;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<ServiceResponse<EnrollmentStudent>> UpdateEnrollmentStatus(int enrollmentID, string status)
        {
            var service = new ServiceResponse<EnrollmentStudent>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@EnrollmentID", enrollmentID);
                param.Add("@EnrollmentStatus", status);
                param.Add("@StatementType", "UPDATESTATUS");

                await conn.ExecuteAsync(
                    "SP_ENROLLMENT",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Message = $"Enrollment status updated to {status}";
                service.Data = new EnrollmentStudent
                {
                    EnrollmentID = enrollmentID,
                    EnrollmentStatus = status
                };
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }
            return service;
        }

       
        public async Task<ServiceResponse<EnrollmentStudent>> ApproveEnrollment(int enrollmentID)
        {
            return await UpdateEnrollmentStatus(enrollmentID, "Approved");
        }

      
        public async Task<ServiceResponse<EnrollmentStudent>> RejectEnrollment(int enrollmentID)
        {
            return await UpdateEnrollmentStatus(enrollmentID, "Rejected");
        }

        public async Task<ServiceResponse<EnrollmentStudent>> CompleteEnrollment(int enrollmentID)
        {
            return await UpdateEnrollmentStatus(enrollmentID, "Enrolled");
        }

        public async Task<ServiceResponse<List<EnrollmentStudent>>> GetEnrollmentsByStudent(int studentID)
        {
            var service = new ServiceResponse<List<EnrollmentStudent>>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", studentID);
                param.Add("@StatementType", "GETBYSTUDENT");

                var result = (await conn.QueryAsync<EnrollmentStudent>(
                    "SP_ENROLLMENT",
                    param,
                    commandType: CommandType.StoredProcedure
                )).ToList();

                service.Status = 200;
                service.Data = result;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }
            return service;
        }

       
        
        public async Task<ServiceResponse<List<EnrollmentStudent>>> GetPendingEnrollments()
        {
            var service = new ServiceResponse<List<EnrollmentStudent>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StatementType", "GETALL");

                var result = (await conn.QueryAsync<EnrollmentStudent>(
                    "SP_ENROLLMENT",
                    param,
                    commandType: CommandType.StoredProcedure
                ))
                .Where(x => x.EnrollmentStatus == "Pending")
                .ToList();

                service.Status = 200;
                service.Data = result;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }







        //new 

        public async Task<ServiceResponse<List<EnrollmentModel>>> GetAllEnrollmentsWithStudentInfoAsync()
        {
            var service = new ServiceResponse<List<EnrollmentModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "GETALL");

               
                var result = await conn.QueryAsync<EnrollmentModel>(
                    "SP_AdminPendingEnrollments",
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
        
        public async Task<ServiceResponse<List<EnrollmentModel>>> GetAdminPendingEnrollmentsAsync()
        {
            var service = new ServiceResponse<List<EnrollmentModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "GETPENDING");

                var result = await conn.QueryAsync<EnrollmentModel>(
                    "SP_AdminPendingEnrollments",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Data = result.ToList();
            
                service.Message = $"Found {result.Count()} pending enrollments";
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
               
            }

            return service;
        }


       
        public async Task<ServiceResponse<List<EnrollmentModel>>> GetApprovedEnrollmentsAsync()
        {
            var service = new ServiceResponse<List<EnrollmentModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "GETAPPROVED");

                var result = await conn.QueryAsync<EnrollmentModel>(
                    "SP_AdminPendingEnrollments",
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



        public async Task<ServiceResponse<EnrollmentModel>> AdminApproveEnrollmentAsync(int enrollmentId)
        {
            var service = new ServiceResponse<EnrollmentModel>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "APPROVE");
                param.Add("@EnrollmentID", enrollmentId);

                var result = await conn.QueryFirstOrDefaultAsync<EnrollmentModel>(
                    "SP_AdminPendingEnrollments",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null)
                {

                  
                    var studentInfo = await GetStudentInfo(result.StudentID);
                    if (!string.IsNullOrEmpty(studentInfo.Email))
                    {
                        await _emailService.SendEnrollmentApprovalEmail(
                            studentInfo.Email,
                            studentInfo.FullName,
                            result.Course,
                            result.SchoolYear,
                            result.Semester
                        );
                    }

                    service.Status = 200;
                    service.Data = result;

                    service.Message = "Enrollment approved successfully";
                }
                else
                {
                    service.Status = 404;
                    service.Message = "Enrollment not found";

                }
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;

            }

            return service;
        }


        // Reject enrollment (updates only TBL_Enrollment)
        public async Task<ServiceResponse<EnrollmentModel>> AdminRejectEnrollmentAsync(int enrollmentId)
        {
            var service = new ServiceResponse<EnrollmentModel>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "REJECT");
                param.Add("@EnrollmentID", enrollmentId);

                var result = await conn.QueryFirstOrDefaultAsync<EnrollmentModel>(
                    "SP_AdminPendingEnrollments",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null)
                {

                    var studentInfo = await GetStudentInfo(result.StudentID);
                    if (!string.IsNullOrEmpty(studentInfo.Email))
                    {
                        await _emailService.SendEnrollmentRejectionEmail(
                            studentInfo.Email,
                            studentInfo.FullName,
                            studentInfo.StudentNumber,
                            "Your enrollment application did not meet the requirements."
                        );
                    }
                    service.Status = 200;
                    service.Data = result;
                   
                    service.Message = "Enrollment rejected";
                }
                else
                {
                    service.Status = 404;
                    service.Message = "Enrollment not found";
                   
                }
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
             
            }

            return service;
        }
        
        private async Task<(string Email, string FullName, string StudentNumber)> GetStudentInfo(int studentId)
        {
            var sql = @"SELECT Email, FirstName + ' ' + LastName AS FullName, StudentNumber 
                FROM TBL_Student WHERE StudentID = @StudentID";
            var result = await conn.QueryFirstOrDefaultAsync<dynamic>(sql, new { StudentID = studentId });

            if (result != null)
            {
                return (result.Email, result.FullName, result.StudentNumber);
            }
            return (string.Empty, string.Empty, string.Empty);
        }
    }
}