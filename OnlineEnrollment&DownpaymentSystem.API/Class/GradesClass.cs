using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data.SqlClient;
using System.Data;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class GradesClass : IGradesRepository
    {
        private readonly SqlConnection conn;

        public GradesClass(IConfiguration config)
        {
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
        }

        public async Task<ServiceResponse<GradesModel>> AddGrade(
    int studentID,
    int enrollmentID,
    int subjectID,
    string schoolYear,
    string semester,
    decimal? grade,
    string remarks)
        {
            var service = new ServiceResponse<GradesModel>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", studentID);
                param.Add("@EnrollmentID", enrollmentID);   
                param.Add("@SubjectID", subjectID);         
                param.Add("@SchoolYear", schoolYear);
                param.Add("@Semester", semester);
                param.Add("@Grade", grade);
                param.Add("@Remarks", remarks);
                param.Add("@StatementType", "INSERT");

                var result = await conn.QueryFirstOrDefaultAsync<GradesModel>(
                    "SP_GRADES",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Message = "Grade added successfully";
                service.Data = result;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<ServiceResponse<GradesModel>> UpdateGrade(int gradeID, decimal? grade, string remarks)
        {
            var service = new ServiceResponse<GradesModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@GradeID", gradeID);
                param.Add("@Grade", grade);
                param.Add("@Remarks", remarks);
                param.Add("@StatementType", "UPDATE");

                var result = await conn.QueryFirstOrDefaultAsync<GradesModel>(
                    "SP_GRADES", param, commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Message = "Grade updated successfully";
                service.Data = result;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }
            return service;
        }

        public async Task<ServiceResponse<List<GradesModel>>> GetGradesByStudent(int studentID)
        {
            var service = new ServiceResponse<List<GradesModel>>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", studentID);
                param.Add("@StatementType", "GETBYSTUDENT");

                var result = (await conn.QueryAsync<GradesModel>(
                    "SP_GRADES", param, commandType: CommandType.StoredProcedure
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

        public async Task<ServiceResponse<GradesModel>> GetGradeByID(int gradeID)
        {
            var service = new ServiceResponse<GradesModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@GradeID", gradeID);
                param.Add("@StatementType", "GETBYID");

                var result = await conn.QueryFirstOrDefaultAsync<GradesModel>(
                    "SP_GRADES", param, commandType: CommandType.StoredProcedure
                );

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
    }
}