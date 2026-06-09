using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data;
using System.Data.SqlClient;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{

    public class EligibilityClass : IEligibilityRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection conn;

        public EligibilityClass(IConfiguration config)
        {
            _configuration = config;
            conn = new SqlConnection(_configuration["ConnectionString:Enrollmentdb"]);
        }
      
        public async Task<ServiceResponse<StudentEligibilityModel>> CheckEligibilityAsync(
            string studentNumber,
            string schoolYear,
            string semester)
        {
            var service = new ServiceResponse<StudentEligibilityModel>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentNumber", studentNumber);
                param.Add("@SchoolYear", schoolYear);
                param.Add("@Semester", semester);

                var result = await conn.QueryFirstOrDefaultAsync<StudentEligibilityModel>(
                    "SP_CheckStudentEnrollmentEligibility",
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
        public async Task<ServiceResponse<List<RegistrarEligibilityModel>>> GetStudentsForRegistrarAsync(string schoolYear, string semester)
        {
            var service = new ServiceResponse<List<RegistrarEligibilityModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@SchoolYear", schoolYear);
                param.Add("@Semester", semester);

                var result = await conn.QueryAsync<RegistrarEligibilityModel>(
                    "SP_GetStudentsForRegistrar",
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

        public async Task<ServiceResponse<bool>> SaveRegistrarEligibilityAsync(RegistrarEligibilityModel eligibility)
        {
            var service = new ServiceResponse<bool>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", eligibility.StudentID);
                param.Add("@SchoolYear", eligibility.SchoolYear);
                param.Add("@Semester", eligibility.Semester);
                param.Add("@IsGradeCleared", eligibility.IsGradeCleared);
                param.Add("@IsClearanceCleared", eligibility.IsClearanceCleared);
                param.Add("@IsPaymentCleared", eligibility.IsPaymentCleared);
                param.Add("@GradeMessage", eligibility.GradeMessage);
                param.Add("@ClearanceMessage", eligibility.ClearanceMessage);
                param.Add("@PaymentMessage", eligibility.PaymentMessage);
                param.Add("@UpdatedBy", 1); 

                await conn.ExecuteAsync(
                    "SP_SaveRegistrarEligibility",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Data = true;
               
                service.Message = "Eligibility saved successfully";
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
